using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PGManagementSystem.Application.Interfaces;
using PGManagementSystem.Domain.Entities;
using PGManagementSystem.Infrastructure.Data;

namespace PGManagementSystem.Infrastructure.Repositories
{
    public class BedRepository : IBedRepository
    {
        private readonly AppDbContext _context;

        public BedRepository(AppDbContext context)
        {
            _context = context;
        }

        // 1. Single Bed by BedId
        public async Task<BedMaster?> GetByIdAsync(Guid bedId)
        {
            return await _context.BedMasters
                .Include(b => b.Zone)
                .FirstOrDefaultAsync(b => b.BedId == bedId);
        }

        // 2. All Beds in a Zone (Room wise allocation ke liye)
        public async Task<List<BedMaster>> GetBedsByZoneIdAsync(Guid zoneId)
        {
            return await _context.BedMasters
                .Where(b => b.ZoneId == zoneId)
                .ToListAsync();
        }

        // 3. All Beds in a Flat (Full Flat allocation ke liye)
        public async Task<List<BedMaster>> GetBedsByFlatIdAsync(Guid flatId)
        {
            return await _context.BedMasters
                .Include(b => b.Zone)
                .Where(b => b.Zone != null && b.Zone.FlatId == flatId)
                .ToListAsync();
        }

        // 4. Add Bed
        public async Task AddAsync(BedMaster bed)
        {
            await _context.BedMasters.AddAsync(bed);
        }

        // 5. Update Bed Status / Tenant Name
        public async Task UpdateAsync(BedMaster bed)
        {
            _context.BedMasters.Update(bed);
            await Task.CompletedTask;
        }

        // 6. Delete Bed
        public async Task DeleteAsync(BedMaster bed)
        {
            _context.BedMasters.Remove(bed);
            await Task.CompletedTask;
        }

        // 7. Commit Changes
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}