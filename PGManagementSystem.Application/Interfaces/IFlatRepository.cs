using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGManagementSystem.Domain.Entities;

namespace PGManagementSystem.Application.Interfaces
{
    public interface IFlatRepository
    {
        Task<bool> IsFlatNumberExistsForOwnerAsync(string flatNumber, Guid userId, Guid? excludeFlatId = null);
        Task AddFlatAsync(FlatMaster flat);
        Task<IEnumerable<FlatMaster>> GetFlatsByUserIdAsync(Guid userId);
        Task<FlatMaster?> GetFlatByIdAsync(Guid id);
        Task UpdateFlatAsync(FlatMaster flat);
        Task DeleteFlatAsync(FlatMaster flat);
        Task<bool> HasOccupiedBedsAsync(Guid flatId);
        void RemoveZone(ZoneMaster zone);
        void RemoveBed(BedMaster bed); 
        void AddZone(ZoneMaster zone);
        void AddBed(BedMaster bed);
    }
}
