using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGManagementSystem.Application.DTOs.Notice
{
    public class UpdateNoticeDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid? FlatId { get; set; }
        public bool IsUrgent { get; set; }
        public bool SendNotification { get; set; }
        public string CreatedByAdminId { get; set; }
    }
}
