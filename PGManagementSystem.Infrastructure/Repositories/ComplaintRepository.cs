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
    public class ComplaintRepository : IComplaintRepository
    {
        private readonly AppDbContext _context;

        public ComplaintRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ComplaintMaster>> GetAllAsync()
        {
            return await _context.Complaints
                .Include(c => c.Flat)
                .Include(c => c.Tenant)
                .Include(c => c.UpdatedByAdmin)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<ComplaintMaster?> GetByIdAsync(Guid complaintId)
        {
            return await _context.Complaints
                .Include(c => c.Flat)
                .Include(c => c.Tenant)
                .Include(c => c.UpdatedByAdmin)
                .FirstOrDefaultAsync(c => c.ComplaintId == complaintId);
        }

        public async Task AddAsync(ComplaintMaster complaint)
        {
            await _context.Complaints.AddAsync(complaint);
        }

        public async Task UpdateAsync(ComplaintMaster complaint)
        {
            _context.Complaints.Update(complaint);
            await Task.CompletedTask;
        }
        public async Task<IEnumerable<ComplaintMaster>> GetByTenantIdAsync(int tenantId)
        {
            return await _context.Complaints
                .Where(c => c.TenantId == tenantId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
