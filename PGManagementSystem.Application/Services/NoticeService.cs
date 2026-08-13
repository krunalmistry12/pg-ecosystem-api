using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PGManagementSystem.Application.DTOs.Notice;
using PGManagementSystem.Application.Interfaces;
using PGManagementSystem.Domain;
using PGManagementSystem.Domain.Entities;

namespace PGManagementSystem.Application.Services
{
    public class NoticeService : INoticeService
    {
        private readonly INoticeRepository _noticeRepository;

        public NoticeService(INoticeRepository noticeRepository)
        {
            _noticeRepository = noticeRepository;
        }

        public async Task<NoticeResponseDto> CreateNoticeAsync(CreateNoticeDto model)
        {
            var notice = new NoticeMaster
            {
                Id = Guid.NewGuid(),
                Title = model.Title,
                Description = model.Description,
                FlatId = model.FlatId,
                IsUrgent = model.IsUrgent,
                SendNotification = model.SendNotification,
                CreatedByAdminId = model.CreatedByAdminId,
                CreatedAt = Global.GetIST()
            };

            var createdNotice = await _noticeRepository.AddAsync(notice);

            return MapToDto(createdNotice);
        }

        public async Task<NoticeResponseDto> UpdateNoticeAsync(Guid id, UpdateNoticeDto model)
        {
            var existingNotice = await _noticeRepository.GetByIdAsync(id);
            if (existingNotice == null) return null;

            existingNotice.Title = model.Title;
            existingNotice.Description = model.Description;
            existingNotice.FlatId = model.FlatId;
            existingNotice.IsUrgent = model.IsUrgent;
            // Agar sendNotification ya koi aur field update karni ho toh yahan add kar sakte hain

            var updatedNotice = await _noticeRepository.UpdateAsync(existingNotice);
            return MapToDto(updatedNotice);
        }

        public async Task<bool> DeleteNoticeAsync(Guid id)
        {
            var existingNotice = await _noticeRepository.GetByIdAsync(id);
            if (existingNotice == null) return false;

            return await _noticeRepository.DeleteAsync(existingNotice);
        }

        public async Task<IEnumerable<NoticeResponseDto>> GetNoticesAsync(Guid? flatId)
        {
            var notices = await _noticeRepository.GetNoticesByPgAsync(flatId);

            return notices.Select(MapToDto).ToList();
        }

        public async Task<IEnumerable<NoticeResponseDto>> GetNoticesByAdminAsync(Guid adminId)
        {
            var notices = await _noticeRepository.GetNoticesByAdminAsync(adminId);

            return notices.Select(MapToDto).ToList();
        }

        private NoticeResponseDto MapToDto(NoticeMaster n)
        {
            var todayUtcDate = DateTime.UtcNow.Date;
            return new NoticeResponseDto
            {
                Id = n.Id.ToString(),
                FlatId = n.FlatId,
                Title = n.Title,
                Desc = n.Description,
                Date = n.CreatedAt.Date == todayUtcDate ? "Today" : n.CreatedAt.ToString("dd MMM yyyy"),
                Urgent = n.IsUrgent
            };
        }
    }
}