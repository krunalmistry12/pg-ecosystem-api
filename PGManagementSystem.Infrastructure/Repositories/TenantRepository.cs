using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PGManagementSystem.Application.DTOs;
using PGManagementSystem.Application.Interfaces;
using PGManagementSystem.Domain.Entities;
using PGManagementSystem.Domain.Enums;
using PGManagementSystem.Infrastructure.Data;

namespace PGManagementSystem.Infrastructure.Repositories
{
    public class TenantRepository : ITenantRepository
    {
        private readonly AppDbContext _context;

        public TenantRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TenantMaster> AddAsync(TenantMaster tenant)
        {
            await _context.TenantMasters.AddAsync(tenant);
            return tenant;
        }

        // FIXED: Added Includes for Flat, Room, Bed so they never return null
        public async Task<TenantMaster?> GetByIdAsync(long id)
        {
            return await _context.TenantMasters
                .Include(t => t.Flat)
                .Include(t => t.Room)
                .Include(t => t.Bed)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<TenantMaster>> GetAllActiveAsync()
        {
            return await _context.TenantMasters
                .Include(t => t.Flat)
                .Include(t => t.Room)
                .Include(t => t.Bed)
                .Where(t => t.Status == enumTenantStatus.ACTIVE)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<TenantMaster>> GetByFlatIdAsync(Guid flatId)
        {
            return await _context.TenantMasters
                .Include(t => t.Flat)
                .Include(t => t.Room)
                .Include(t => t.Bed)
                .Where(t => t.FlatId == flatId)
                .AsNoTracking()
                .ToListAsync();
        }

        public void Update(TenantMaster tenant)
        {
            _context.TenantMasters.Update(tenant);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<List<TenantMaster>> GetTenantsByUserIdAsync(Guid userId)
        {
            return await _context.TenantMasters
                .Include(t => t.Flat)
                .Include(t => t.Room)
                .Include(t => t.Bed)
                .Where(t => t.Flat.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}