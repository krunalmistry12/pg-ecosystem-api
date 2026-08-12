using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGManagementSystem.Application.DTOs.Notice;

namespace PGManagementSystem.Application.Interfaces
{
    public interface INoticeService
    {
        Task<NoticeResponseDto> CreateNoticeAsync(CreateNoticeDto model);
        Task<IEnumerable<NoticeResponseDto>> GetNoticesAsync(Guid? flatid);
        Task<IEnumerable<NoticeResponseDto>> GetNoticesByAdminAsync(Guid id);
    }
}
