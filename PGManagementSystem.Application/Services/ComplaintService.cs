using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGManagementSystem.Application.DTOs.Complaint;
using PGManagementSystem.Application.Interfaces;
using PGManagementSystem.Domain.Entities;

namespace PGManagementSystem.Application.Services
{
    public class ComplaintService
    {
        private readonly IComplaintRepository _complaintRepo;

        public ComplaintService(IComplaintRepository complaintRepo)
        {
            _complaintRepo = complaintRepo;
        }

        //// 1. Get all complaints (For Admin Dashboard)
        //public async Task<IEnumerable<ComplaintMaster>> GetAllComplaintsAsync()
        //{
        //    return await _complaintRepo.GetAllAsync();
        //}

        // 2. Tenant creates a new complaint
        public async Task<ComplaintMaster> CreateComplaintAsync(CreateComplaintDto dto)
        {
            var complaint = new ComplaintMaster
            {
                ComplaintId = Guid.NewGuid(),
                FlatId = dto.FlatId,
                TenantId = dto.TenantId,
                Title = dto.Title,
                Category = dto.Category,
                Priority = dto.Priority,
                Status = "Pending", // Default status when tenant creates
                AttachmentName = dto.AttachmentName,
                AttachmentUri = dto.AttachmentUri,
                ComplaintDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            await _complaintRepo.AddAsync(complaint);
            await _complaintRepo.SaveChangesAsync();

            return complaint;
        }

        // 3. Admin updates complaint status & remark
        public async Task<ComplaintMaster?> UpdateComplaintStatusAsync(UpdateComplaintDto dto,Guid UpdatedByUserId)
        {
            var complaint = await _complaintRepo.GetByIdAsync(dto.ComplaintId);
            if (complaint == null)
            {
                return null;
            }

            complaint.Status = dto.Status;
            complaint.AdminRemark = dto.AdminRemark;
            complaint.UpdatedByUserId = UpdatedByUserId;
            complaint.UpdatedAt = DateTime.UtcNow;

            await _complaintRepo.UpdateAsync(complaint);
            await _complaintRepo.SaveChangesAsync();

            return complaint;
        }
        public async Task<IEnumerable<ComplaintResponseDto>> GetComplaintsByTenantIdAsync(int tenantId)
        {
            var complaints = await _complaintRepo.GetByTenantIdAsync(tenantId);

            return complaints.Select(c => new ComplaintResponseDto
            {
                ComplaintId = c.ComplaintId,
                TenantId = c.TenantId,
                Title = c.Title,
                Description = c.Description,
                Status = c.Status.ToString(), // Enum ko string mein convert karna
                AdminRemark = c.AdminRemark,  // Admin ka diya hua remark
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            });
        }

        // Saari complaints dekhne ke liye (Admin ke liye)
        public async Task<IEnumerable<ComplaintResponseDto>> GetAllComplaintsAsync()
        {
            var complaints = await _complaintRepo.GetAllAsync();

            return complaints.Select(c => new ComplaintResponseDto
            {
                ComplaintId = c.ComplaintId,
                TenantId = c.TenantId,
                TenantName = c.Tenant?.Name, // Tenant ka naam agar nav property load ho
                Title = c.Title,
                Description = c.Description,
                Status = c.Status.ToString(),
                AdminRemark = c.AdminRemark,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            });
        }
    }
}
