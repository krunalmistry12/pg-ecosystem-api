using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGManagementSystem.Domain.Entities
{
    [Table("COMPLAINT_MST")]
    public class ComplaintMaster
    {
        [Key]
        public Guid ComplaintId { get; set; } = Guid.NewGuid();

        // --- Flat Mapping (Kis Flat / Room ki complaint hai) ---
        [Required]
        public Guid FlatId { get; set; }

        [ForeignKey("FlatId")]
        public FlatMaster? Flat { get; set; }

        // --- Tenant Mapping (Kis tenant ne raise kiya hai) ---
        [Required]
        public long TenantId { get; set; }

        [ForeignKey("TenantId")]
        public TenantMaster? Tenant { get; set; }

        // --- Complaint Details ---
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty; // Plumbing, Internet, Electronics, etc.

        [Required]
        [MaxLength(20)]
        public string Priority { get; set; } = "Medium"; // High, Medium, Low

        [Required]
        [MaxLength(30)]
        public string Status { get; set; } = "Pending"; // Pending, In Progress, Resolved

        [MaxLength(500)]
        public string? AdminRemark { get; set; } // Admin ka action note

        // --- Optional Attachment (Photo/Proof) ---
        [MaxLength(255)]
        public string? AttachmentName { get; set; }

        [MaxLength(500)]
        public string? AttachmentUri { get; set; }

        // --- Audit / Update Tracking (Kis Admin ne update kiya) ---
        public Guid? UpdatedByUserId { get; set; }

        [ForeignKey("UpdatedByUserId")]
        public UserMaster? UpdatedByAdmin { get; set; }

        [Required]
        public DateTime ComplaintDate { get; set; } = DateTime.UtcNow;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
