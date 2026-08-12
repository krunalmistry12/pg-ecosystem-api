using System;

namespace PGManagementSystem.Application.DTOs.Notice
{
    public class NoticeResponseDto
    {
        public string Id { get; set; } = string.Empty;

        public Guid? FlatId { get; set; }

        public string PgName { get; set; } = "All PGs (Common)";

        public string Title { get; set; } = string.Empty;
        public string Desc { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty; 
        public bool Urgent { get; set; }
    }
}