using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGManagementSystem.Application.DTOs.Complaint
{
    public class CreateComplaintDto
    {
        [Required]
        public Guid FlatId { get; set; }

        [Required]
        public int TenantId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty; // Plumbing, Internet, etc.

        [Required]
        [MaxLength(20)]
        public string Priority { get; set; } = "Medium"; // High, Medium, Low

        [MaxLength(255)]
        public string? AttachmentName { get; set; }

        [MaxLength(500)]
        public string? AttachmentUri { get; set; }
    }
}