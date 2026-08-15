using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGManagementSystem.Domain.Entities;

namespace PGManagementSystem.Application.Interfaces
{
    public interface IComplaintRepository
    {
        Task<IEnumerable<ComplaintMaster>> GetAllAsync();
        Task<ComplaintMaster?> GetByIdAsync(Guid complaintId);
        Task AddAsync(ComplaintMaster complaint);
        Task UpdateAsync(ComplaintMaster complaint);
        Task SaveChangesAsync();
        Task<IEnumerable<ComplaintMaster>> GetByTenantIdAsync(int tenantId);
    }
}
