using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGManagementSystem.Domain.Enums;

namespace PGManagementSystem.Domain.Entities
{
    [Table("BED_MST")]
    public class BedMaster
    {
        [Key]
        public Guid BedId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        public string BedNumber { get; set; } = string.Empty; // "Bed 1", "Bed 2"

        [Required]
        [MaxLength(30)]
        public enumBedStatus Status { get; set; } = enumBedStatus.Vacant; // "vacant" | "occupied" | "reserved" | "maintenance"

        [MaxLength(100)]
        public string? TenantName { get; set; } // Optional Tenant Name

        // --- Foreign Key to ZoneMaster ---
        [Required]
        public Guid ZoneId { get; set; }

        [ForeignKey("ZoneId")]
        public ZoneMaster? Zone { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal BedRent { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
