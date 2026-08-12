using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGManagementSystem.Domain.Entities;

namespace PGManagementSystem.Application.Interfaces
{
    public interface INoticeRepository
    {
        Task<NoticeMaster> AddAsync(NoticeMaster notice);
        Task<IEnumerable<NoticeMaster>> GetNoticesByPgAsync(Guid? flatid);
        Task<IEnumerable<NoticeMaster>> GetNoticesByAdminAsync(Guid flatid);
    }
}
