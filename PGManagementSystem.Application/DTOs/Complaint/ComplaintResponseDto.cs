using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGManagementSystem.Application.DTOs.Complaint
{
    public class ComplaintResponseDto
    {
        public Guid ComplaintId { get; set; }
        public long TenantId { get; set; }
        public string? TenantName { get; set; } // Optional: Agar tenant ka naam dikhana ho
        public string? Phone { get; set; } // Optional: Agar tenant ka naam dikhana ho
        public string? Room { get; set; } // Optional: Agar tenant ka naam dikhana ho
        public string? pgName { get; set; } // Optional: Agar tenant ka naam dikhana ho
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // e.g., Pending, InProgress, Resolved
        public string? AdminRemark { get; set; } // Admin ka reply/remark
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
