using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGManagementSystem.Domain.Entities;

namespace PGManagementSystem.Application.Interfaces
{
    public interface IBedRepository
    {
        Task<BedMaster?> GetByIdAsync(Guid bedId);
        Task<List<BedMaster>> GetBedsByZoneIdAsync(Guid zoneId);
        Task<List<BedMaster>> GetBedsByFlatIdAsync(Guid flatId);
        Task AddAsync(BedMaster bed);
        Task UpdateAsync(BedMaster bed);
        Task DeleteAsync(BedMaster bed);
        Task SaveChangesAsync();
    }
}
