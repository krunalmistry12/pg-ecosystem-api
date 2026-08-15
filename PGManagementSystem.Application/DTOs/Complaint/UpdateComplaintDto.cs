using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGManagementSystem.Application.DTOs.Complaint
{
    public class UpdateComplaintDto
    {
        [Required]
        public Guid ComplaintId { get; set; }

        [Required]
        [MaxLength(30)]
        public string Status { get; set; } = string.Empty; 

        [MaxLength(500)]
        public string? AdminRemark { get; set; }

    }
}
