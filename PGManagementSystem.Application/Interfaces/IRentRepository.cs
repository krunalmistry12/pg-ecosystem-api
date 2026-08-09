using System.Collections.Generic;
using System.Threading.Tasks;
using PGManagementSystem.Domain.Entities;

namespace PGManagementSystem.Application.Interfaces
{
    public interface IRentRepository
    {
        Task<RentMaster?> GetByIdAsync(long rentId);
        Task<RentMaster?> GetRentByTenantAndMonthAsync(long tenantId, int month, int year);
        Task<List<RentMaster>> GetPendingBillsAsync();
        Task AddRentAsync(RentMaster rent);
        Task AddPaymentHistoryAsync(RentPaymentHistory payment);
        Task<List<RentPaymentHistory>> GetPaymentHistoryByRentIdAsync(long rentId);
        Task SaveChangesAsync();

        // 👈 TENANT SIDE
        Task<List<RentMaster>> GetPendingBillsByTenantIdAsync(long tenantId);
        Task<List<RentMaster>> GetAllRentsByTenantIdAsync(long tenantId); // Full bill history for Tenant
        Task<List<RentPaymentHistory>> GetPaymentHistoryByTenantIdAsync(long tenantId);

        // 👈 PG OWNER SIDE (SaaS Multi-tenant Data Isolation)
        Task<List<RentMaster>> GetRentsByOwnerIdAsync(Guid ownerId);
    }
}