using Microsoft.EntityFrameworkCore;
using PGManagementSystem.Domain.Entities;
using PGManagementSystem.Domain.Enums;

namespace PGManagementSystem.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // ==========================================
    // DB SETS (Database Tables)
    // ==========================================
    public DbSet<RoleMaster> RoleMasters { get; set; } = null!;
    public DbSet<UserMaster> UserMasters { get; set; } = null!;
    public DbSet<FlatMaster> FlatMasters { get; set; } = null!;
    public DbSet<ZoneMaster> ZoneMasters { get; set; } = null!;
    public DbSet<BedMaster> BedMasters { get; set; } = null!;
    public DbSet<TenantMaster> TenantMasters { get; set; } = null!;
    public DbSet<RentMaster> RentMasters { get; set; } = null!;
    public DbSet<RentPaymentHistory> RentPaymentHistories { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ==========================================
        // RENT MANAGEMENT RELATIONSHIPS
        // ==========================================

        // Tenant (1) -> RentMaster (N)
        modelBuilder.Entity<RentMaster>()
            .HasOne(r => r.Tenant)
            .WithMany(t => t.Rents)
            .HasForeignKey(r => r.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        // RentMaster (1) -> RentPaymentHistory (N)
        modelBuilder.Entity<RentPaymentHistory>()
            .HasOne(p => p.Rent)
            .WithMany(r => r.PaymentHistories)
            .HasForeignKey(p => p.RentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Unique constraint: Ek tenant ka ek month-year me ek hi rent bill bane
        modelBuilder.Entity<RentMaster>()
            .HasIndex(r => new { r.TenantId, r.BillingMonth, r.BillingYear })
            .IsUnique();

        // ==========================================
        // 1. UNIQUE INDEXES & CONSTRAINTS
        // ==========================================

        // UserMaster: Email and Phone unique safeguard at DB level
        modelBuilder.Entity<UserMaster>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<UserMaster>()
            .HasIndex(u => u.Phone)
            .IsUnique();

        // FlatMaster: Same Owner duplicate Flat Number add na kar sake
        modelBuilder.Entity<FlatMaster>()
            .HasIndex(f => new { f.FlatNumber, f.UserId })
            .IsUnique();

        // ==========================================
        // 2. ENUM CONVERSIONS (Stored as Text in DB)
        // ==========================================

        // BedStatus enum -> "Vacant", "Occupied", "Reserved", "Maintenance"
        modelBuilder.Entity<BedMaster>()
            .Property(b => b.Status)
            .HasConversion<string>();

        // ZoneType enum -> "NonAC", "AC"
        modelBuilder.Entity<ZoneMaster>()
            .Property(z => z.Type)
            .HasConversion<string>();

        // ==========================================
        // 3. RELATIONSHIPS & CASCADE DELETE CONFIG
        // ==========================================

        // RoleMaster (1) -> UserMaster (N)
        modelBuilder.Entity<UserMaster>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        // UserMaster (1) -> FlatMaster (N)
        modelBuilder.Entity<FlatMaster>()
            .HasOne(f => f.Owner)
            .WithMany(u => u.Flats)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // FlatMaster (1) -> ZoneMaster (N)
        modelBuilder.Entity<ZoneMaster>()
            .HasOne(z => z.Flat)
            .WithMany(f => f.Zones)
            .HasForeignKey(z => z.FlatId)
            .OnDelete(DeleteBehavior.Cascade);

        // ZoneMaster (1) -> BedMaster (N)
        modelBuilder.Entity<BedMaster>()
            .HasOne(b => b.Zone)
            .WithMany(z => z.Beds)
            .HasForeignKey(b => b.ZoneId)
            .OnDelete(DeleteBehavior.Cascade);

        // ==========================================
        // 4. DEFAULT SEED DATA (Default Roles)
        // ==========================================
        modelBuilder.Entity<RoleMaster>().HasData(
            new RoleMaster
            {
                RoleId = 1,
                RoleName = "SuperAdmin",
                Description = "System Administrator with full access",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new RoleMaster
            {
                RoleId = 2,
                RoleName = "Admin",
                Description = "PG Owner / Main Admin",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new RoleMaster
            {
                RoleId = 3,
                RoleName = "Staff",
                Description = "PG Manager / Maintenance Staff",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new RoleMaster
            {
                RoleId = 4,
                RoleName = "Tenant",
                Description = "PG Tenant / Guest",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}