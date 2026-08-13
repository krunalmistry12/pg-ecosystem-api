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
    public class NoticeRepository : INoticeRepository
    {
        private readonly AppDbContext _context;

        public NoticeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<NoticeMaster> AddAsync(NoticeMaster notice)
        {
            _context.NoticeMasters.Add(notice);
            await _context.SaveChangesAsync();
            return notice;
        }

        public async Task<IEnumerable<NoticeMaster>> GetNoticesByPgAsync(Guid? flatId)
        {
            var query = _context.NoticeMasters.AsQueryable();

            if (flatId.HasValue && flatId != Guid.Empty)
            {
                query = query.Where(n => n.FlatId == flatId || n.FlatId == null);
            }

            return await query.OrderByDescending(n => n.CreatedAt).ToListAsync();
        }
        public async Task<IEnumerable<NoticeMaster>> GetNoticesByAdminAsync(Guid Id)
        {
            var query = _context.NoticeMasters.AsQueryable();

            if (Id != Guid.Empty)
            {
                query = query.Where(n => n.CreatedByAdminId == Id.ToString() || n.CreatedByAdminId == null);
            }

            return await query.OrderByDescending(n => n.CreatedAt).ToListAsync();
        }
        public async Task<NoticeMaster> GetByIdAsync(Guid id)
        {
            return await _context.NoticeMasters.FindAsync(id);
        }
        public async Task<NoticeMaster> UpdateAsync(NoticeMaster notice)
        {
            _context.NoticeMasters.Update(notice);
            await _context.SaveChangesAsync();
            return notice;
        }

        public async Task<bool> DeleteAsync(NoticeMaster notice)
        {
            _context.NoticeMasters.Remove(notice);
            var affectedRows = await _context.SaveChangesAsync();
            return affectedRows > 0;
        }
    }
}
