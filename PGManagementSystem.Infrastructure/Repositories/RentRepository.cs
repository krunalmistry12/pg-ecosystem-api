using Microsoft.EntityFrameworkCore;
using PGManagementSystem.Application.Interfaces;
using PGManagementSystem.Domain.Entities;
using PGManagementSystem.Domain.Enums;
using PGManagementSystem.Infrastructure.Data;

namespace PGManagementSystem.Infrastructure.Repositories;

public class RentRepository : IRentRepository
{
    private readonly AppDbContext _context;

    public RentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<RentMaster?> GetByIdAsync(long rentId)
    {
        return await _context.RentMasters
            .Include(r => r.Tenant)
            .FirstOrDefaultAsync(r => r.Id == rentId);
    }

    public async Task<RentMaster?> GetRentByTenantAndMonthAsync(long tenantId, int month, int year)
    {
        return await _context.RentMasters
            .FirstOrDefaultAsync(r => r.TenantId == tenantId && r.BillingMonth == month && r.BillingYear == year);
    }

    public async Task<List<RentMaster>> GetPendingBillsAsync()
    {
        return await _context.RentMasters
            .Include(r => r.Tenant)
            .Where(r => r.Status == enumPaymentStatus.PENDING || r.Status == enumPaymentStatus.PARTIAL || r.Status == enumPaymentStatus.OVERDUE)
            .ToListAsync();
    }

    public async Task AddRentAsync(RentMaster rent)
    {
        await _context.RentMasters.AddAsync(rent);
    }

    public async Task AddPaymentHistoryAsync(RentPaymentHistory payment)
    {
        await _context.RentPaymentHistories.AddAsync(payment);
    }

    public async Task<List<RentPaymentHistory>> GetPaymentHistoryByRentIdAsync(long rentId)
    {
        return await _context.RentPaymentHistories
            .Where(p => p.RentId == rentId)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    // -------------------------------------------------------------
    // 👈 TENANT QUERIES
    // -------------------------------------------------------------
    public async Task<List<RentMaster>> GetPendingBillsByTenantIdAsync(long tenantId)
    {
        return await _context.RentMasters
            .Include(r => r.Tenant)
            .AsNoTracking()
            .Where(r => r.TenantId == tenantId && r.Status != enumPaymentStatus.PAID)
            .OrderByDescending(r => r.BillingYear)
            .ThenByDescending(r => r.BillingMonth)
            .ToListAsync();
    }

    // Tenant ki saari past & current bills (PAID, PENDING, PARTIAL)
    public async Task<List<RentMaster>> GetAllRentsByTenantIdAsync(long tenantId)
    {
        return await _context.RentMasters
            .Include(r => r.Tenant)
            .AsNoTracking()
            .Where(r => r.TenantId == tenantId)
            .OrderByDescending(r => r.BillingYear)
            .ThenByDescending(r => r.BillingMonth)
            .ToListAsync();
    }

    public async Task<List<RentPaymentHistory>> GetPaymentHistoryByTenantIdAsync(long tenantId)
    {
        return await _context.RentPaymentHistories
            .Include(ph => ph.Rent)
            .AsNoTracking()
            .Where(ph => ph.Rent.TenantId == tenantId)
            .OrderByDescending(ph => ph.PaymentDate)
            .ToListAsync();
    }

    // -------------------------------------------------------------
    // 👈 PG OWNER QUERIES (Owner -> Flat -> Tenant -> RentMaster)
    // -------------------------------------------------------------
    public async Task<List<RentMaster>> GetRentsByOwnerIdAsync(Guid ownerId)
    {
        return await _context.RentMasters
            .Include(r => r.Tenant)
                .ThenInclude(t => t.Flat)
                .Include(r => r.PaymentHistories)
            .AsNoTracking()
            .Where(r => r.Tenant != null && r.Tenant.Flat != null && r.Tenant.Flat.UserId == ownerId)
            .OrderByDescending(r => r.BillingYear)
            .ThenByDescending(r => r.BillingMonth)
            .ThenByDescending(r => r.CreatedAt)
            .ToListAsync();
    }
}