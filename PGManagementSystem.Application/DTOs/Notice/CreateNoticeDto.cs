using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGManagementSystem.Application.DTOs.Notice
{
    public class CreateNoticeDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty; 
        public Guid? FlatId { get; set; } 
        public bool IsUrgent { get; set; } = false;
        public bool SendNotification { get; set; } = true;
        public string CreatedByAdminId { get; set; } = string.Empty;
    }
}
