using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PGManagementSystem.Application.DTOs.Dashboard;
using PGManagementSystem.Application.Interfaces;
using PGManagementSystem.Domain.Enums;
using PGManagementSystem.Infrastructure.Data;

namespace PGManagementSystem.Infrastructure.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AppDbContext _context;
        private static readonly TimeZoneInfo IndianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        public DashboardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardResponseDto> GetDashboardDataAsync(Guid userId, string role, int? month, int? year)
        {
            // 1. IST Time Setup
            DateTime nowUtc = DateTime.UtcNow;
            DateTime nowIst = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, IndianTimeZone);

            int targetMonth = month ?? nowIst.Month;
            int targetYear = year ?? nowIst.Year;
            string monthName = new DateTime(targetYear, targetMonth, 1).ToString("MMMM");

            bool isPastMonth = (targetYear < nowIst.Year) || (targetYear == nowIst.Year && targetMonth < nowIst.Month);
            bool isCurrentMonth = (targetYear == nowIst.Year && targetMonth == nowIst.Month);

            // 2. Base Query Filters based on User Role & Id
            var flatsQuery = _context.FlatMasters.AsQueryable();
            var zonesQuery = _context.ZoneMasters.AsQueryable();
            var bedsQuery = _context.BedMasters.AsQueryable();
            var tenantsQuery = _context.TenantMasters.AsQueryable();

            if (role != "SuperAdmin")
            {
                flatsQuery = flatsQuery.Where(f => f.UserId == userId);
                zonesQuery = zonesQuery.Where(z => flatsQuery.Any(f => f.FlatId == z.FlatId));
                bedsQuery = bedsQuery.Where(b => zonesQuery.Any(z => z.ZoneId == b.ZoneId));
                tenantsQuery = tenantsQuery.Where(t => flatsQuery.Any(f => f.FlatId == t.FlatId));
            }

            // 3. Property Metrics Calculations
            int totalRooms = await zonesQuery.CountAsync();

            int occupiedRooms = await tenantsQuery
                .Where(t => t.Status == enumTenantStatus.ACTIVE && t.RoomId != null)
                .Select(t => t.RoomId)
                .Distinct()
                .CountAsync();

            int vacantRooms = Math.Max(0, totalRooms - occupiedRooms);
            int activeTenants = await tenantsQuery.CountAsync(t => t.Status == enumTenantStatus.ACTIVE);

            var startOfMonth = new DateTime(targetYear, targetMonth, 1);
            var endOfMonth = startOfMonth.AddMonths(1);

            int newJoiners = await tenantsQuery
                .CountAsync(t => t.JoiningDate >= startOfMonth && t.JoiningDate < endOfMonth);

            // =========================================================================
            // 💰 4. RENT & REVENUE CALCULATIONS (Using RentMasters for Accuracy)
            // =========================================================================

            // Target month ke generate huye saare bills ka total amount (Expected Revenue)
            decimal totalExpected = await _context.RentMasters
                .Where(r => flatsQuery.Any(f => f.FlatId == r.FlatId) &&
                            r.BillingMonth == targetMonth &&
                            r.BillingYear == targetYear)
                .SumAsync(r => (decimal?)(r.BaseRent + r.ElectricityBill + r.ExtraCharges + r.LateFee - r.Discount)) ?? 0;

            // Fallback: Agar us mahine ke bills generate nahi huye hain aur current month hai, toh active tenants ke rent ko sum karein
            if (totalExpected == 0 && isCurrentMonth)
            {
                totalExpected = await tenantsQuery
                    .Where(t => t.Status == enumTenantStatus.ACTIVE)
                    .SumAsync(t => (decimal?)t.Rent) ?? 0;
            }

            // Total Collected amount using RentMaster's PaidAmount property
            decimal totalCollected = await _context.RentMasters
                .Where(r => flatsQuery.Any(f => f.FlatId == r.FlatId) &&
                            r.BillingMonth == targetMonth &&
                            r.BillingYear == targetYear &&
                            (r.Status == enumPaymentStatus.PAID || (int)r.Status == 1) || (int)r.Status == 3)
                .SumAsync(r => (decimal?)r.PaidAmount) ?? 0;

            decimal totalPendingDue = totalExpected > totalCollected ? totalExpected - totalCollected : 0;

            // =========================================================================
            // 🚨 5. ALERTS GENERATION
            // =========================================================================
            var alerts = new List<DashboardAlertDto>();

            var paidTenantIdsQuery = _context.RentMasters
                .Where(r => flatsQuery.Any(f => f.FlatId == r.FlatId) &&
                            r.BillingMonth == targetMonth &&
                            r.BillingYear == targetYear &&
                            (r.Status == enumPaymentStatus.PAID || (int)r.Status == 1))
                .Select(r => r.TenantId);

            int overdueCount = await tenantsQuery
                .Where(t => t.Status == enumTenantStatus.ACTIVE &&
                            !paidTenantIdsQuery.Contains(t.Id) &&
                            (isPastMonth || (isCurrentMonth && t.DueDate < nowIst.Day)))
                .CountAsync();

            if (overdueCount > 0)
            {
                alerts.Add(new DashboardAlertDto
                {
                    Id = "rent_alert_1",
                    Type = "rent",
                    Title = "Action Required: Rent Dues",
                    Subtitle = $"{overdueCount} tenant(s) have pending rent past their due date for {monthName}.",
                    Time = "Live",
                    Route = "/rent",
                    Color = "#EF4444"
                });
            }

            // 6. Recent Activities
            var recentTenants = await tenantsQuery
                .OrderByDescending(t => t.CreatedAt)
                .Take(5)
                .ToListAsync();

            var recentActivities = recentTenants.Select(t => new RecentActivityDto
            {
                Id = t.Id.ToString(),
                Text = $"{t.Name} joined with rent ₹{t.Rent:N0}",
                Time = GetTimeAgo(t.CreatedAt, nowIst),
                Icon = "person-add-outline",
                Color = "#10B981"
            }).ToList();

            int occupancyPercentage = totalRooms > 0 ? (int)((double)occupiedRooms / totalRooms * 100) : 0;

            string ownerName = await _context.UserMasters
                                .Where(u => u.UserId == userId)
                                .Select(u => u.FullName)
                                .FirstOrDefaultAsync() ?? "";

            return new DashboardResponseDto
            {
                OwnerName = ownerName,
                RevenueOverview = new RevenueOverviewDto
                {
                    MonthName = monthName,
                    TotalExpectedRevenue = totalExpected,
                    TotalCollected = totalCollected,
                    TotalPendingDue = totalPendingDue
                },
                PropertyMetrics = new PropertyMetricsDto
                {
                    TotalRooms = totalRooms,
                    OccupiedRooms = occupiedRooms,
                    VacantRooms = vacantRooms,
                    ActiveTenants = activeTenants,
                    NewJoinersThisMonth = newJoiners,
                    OccupancyPercentage = occupancyPercentage
                },
                Alerts = alerts,
                RecentActivities = recentActivities
            };
        }

        private string GetTimeAgo(DateTime createdAtUtc, DateTime nowIst)
        {
            var createdIst = TimeZoneInfo.ConvertTimeFromUtc(createdAtUtc, IndianTimeZone);
            var span = nowIst - createdIst;

            if (span.TotalMinutes < 1) return "Just now";
            if (span.TotalMinutes < 60) return $"{(int)span.TotalMinutes} mins ago";
            if (span.TotalHours < 24) return $"{(int)span.TotalHours} hours ago";
            if (span.TotalDays < 30) return $"{(int)span.TotalDays} days ago";
            return createdIst.ToString("dd MMM yyyy");
        }
    }
}