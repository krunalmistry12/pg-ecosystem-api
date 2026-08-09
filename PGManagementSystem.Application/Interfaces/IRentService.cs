using System.Collections.Generic;
using System.Threading.Tasks;
using PGManagementSystem.Application.DTOs.Rent;
using PGManagementSystem.Domain.Entities;

namespace PGManagementSystem.Application.Interfaces
{
    public interface IRentService
    {
        Task<RentBillResponseDto> GenerateRentBillAsync(GenerateRentBillDto dto);
        Task<PaymentReceiptResponseDto> RecordPaymentAsync(RecordPaymentDto dto);

        // Admin Methods
        Task<List<RentBillResponseDto>> GetPendingRentBillsAsync();
        Task<List<RentPaymentHistory>> GetPaymentHistoryByRentIdAsync(long rentId);
        Task<List<RentBillResponseDto>> GetAllRentRecordsForAdminAsync(Guid ownerId, int? month = null, int? year = null, string? status = null, string? search = null);

        // Tenant Methods
        Task<List<RentBillResponseDto>> GetPendingBillsByTenantIdAsync(long tenantId);
        Task<List<RentBillResponseDto>> GetAllRentsByTenantIdAsync(long tenantId);
        Task<List<RentPaymentHistory>> GetPaymentHistoryByTenantIdAsync(long tenantId);
    }
}