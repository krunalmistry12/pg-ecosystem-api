using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PGManagementSystem.Application.DTOs;
using PGManagementSystem.Domain.Entities;

namespace PGManagementSystem.Application.Interfaces
{
    public interface ITenantRepository
    {
        Task<TenantMaster> AddAsync(TenantMaster tenant);
        Task<TenantMaster?> GetByIdAsync(long id);
        Task<IEnumerable<TenantMaster>> GetAllActiveAsync();
        Task<IEnumerable<TenantMaster>> GetByFlatIdAsync(Guid flatId);
        Task<List<TenantMaster>> GetTenantsByUserIdAsync(Guid userId);
        void Update(TenantMaster tenant);
        Task<bool> SaveChangesAsync();
    }
}