using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using PGManagementSystem.Application.Interfaces;
using PGManagementSystem.Domain.Entities;
using PGManagementSystem.Domain.Enums;
using PGManagementSystem.Infrastructure.Data;

namespace PGManagementSystem.Infrastructure.Repositories
{
    public class FlatRepository : IFlatRepository
    {
        private readonly AppDbContext _context;

        public FlatRepository(AppDbContext context)
        {
            _context = context;
        }

        // 1. Get All Flats by User/Owner ID with Zones & Beds
        public async Task<IEnumerable<FlatMaster>> GetFlatsByUserIdAsync(Guid userId)
        {
            return await _context.FlatMasters
                .Where(f => f.UserId == userId)
                .Include(f => f.Zones)
                    .ThenInclude(z => z.Beds)
                .AsNoTracking()
                .ToListAsync();
        }
        public void AddZone(ZoneMaster zone)
        {
            _context.ZoneMasters.Add(zone); // Explicitly marks as Added (INSERT)
        }

        public void AddBed(BedMaster bed)
        {
            _context.BedMasters.Add(bed); // Explicitly marks as Added (INSERT)
        }

        // 2. Get Single Flat by Flat ID
        public async Task<FlatMaster?> GetFlatByIdAsync(Guid id)
        {
            return await _context.FlatMasters
                .Include(f => f.Zones)
                    .ThenInclude(z => z.Beds)
                .FirstOrDefaultAsync(f => f.FlatId == id);
        }

        // 3. Create / Add Flat
        public async Task AddFlatAsync(FlatMaster flat)
        {
            await _context.FlatMasters.AddAsync(flat);
            await _context.SaveChangesAsync();
        }

        // 4. Update Flat
        public async Task UpdateFlatAsync(FlatMaster flat)
        {
            try
            {
                // DB Changes Save karne ki koshish
                await _context.SaveChangesAsync();
                
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw;
            }
        }
        public void RemoveZone(ZoneMaster zone)
        {
            _context.ZoneMasters.Remove(zone);
        }

        public void RemoveBed(BedMaster bed)
        {
            _context.BedMasters.Remove(bed);
        }
        // 5. Delete Flat
        public async Task DeleteFlatAsync(FlatMaster flat)
        {
            _context.FlatMasters.Remove(flat);
            await _context.SaveChangesAsync();
        }

        // 6. Check if any Bed in this Flat is Occupied or Reserved
        public async Task<bool> HasOccupiedBedsAsync(Guid flatId)
        {
            return await _context.ZoneMasters
                .Where(z => z.FlatId == flatId)
                .SelectMany(z => z.Beds)
                .AnyAsync(b => b.Status == enumBedStatus.Occupied || b.Status == enumBedStatus.Reserved);
        }

        // 7. Check Duplicate Flat Number for Same Owner
        // 7. Check Duplicate Flat Number for Same Owner (With optional Exclude ID for Updates)
        public async Task<bool> IsFlatNumberExistsForOwnerAsync(string flatNumber, Guid userId, Guid? excludeFlatId = null)
        {
            if (string.IsNullOrWhiteSpace(flatNumber)) return false;

            var cleanFlatNumber = flatNumber.Trim().ToLower();

            return await _context.FlatMasters
                .AnyAsync(f => f.UserId == userId
                            && f.FlatNumber.ToLower() == cleanFlatNumber
                            && (!excludeFlatId.HasValue || f.FlatId != excludeFlatId.Value));
        }
    }
}