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
    [Table("ZONE_MST")]
    public class ZoneMaster
    {
        [Key]
        public Guid ZoneId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string ZoneName { get; set; } = string.Empty; // "Hall Space", "Bedroom 1"

        [Required]
        [MaxLength(20)]
        public enumZoneType Type { get; set; } = enumZoneType.NonAC; // "AC" or "Non AC"

        public int Capacity { get; set; } // Total beds count

        [Column(TypeName = "decimal(18,2)")]
        public decimal RentPerBed { get; set; } // Rent amount

        // --- Foreign Key to FlatMaster ---
        [Required]
        public Guid FlatId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? RoomRent { get; set; }
        [ForeignKey("FlatId")]
        public FlatMaster? Flat { get; set; }

        // One Zone has Multiple Beds
        public ICollection<BedMaster> Beds { get; set; } = new List<BedMaster>();
    }
}
