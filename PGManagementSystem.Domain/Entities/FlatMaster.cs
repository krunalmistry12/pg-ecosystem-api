using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGManagementSystem.Domain.Entities
{
    [Table("FLAT_MST")]
    public class FlatMaster
    {
        [Key]
        public Guid FlatId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        public string FlatNumber { get; set; } = string.Empty; // e.g. "101"

        [Required]
        [MaxLength(150)]
        public string ApartmentName { get; set; } = string.Empty; // e.g. "Roma Apartment"

        // --- Foreign Key to UserMaster (Admin / PG Owner) ---
        [Required]
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public UserMaster? Owner { get; set; }
        [Required]
        public string PricingType { get; set; } = "BED_WISE"; // BED_WISE | ROOM_WISE
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // One Flat has Multiple Zones
        public ICollection<ZoneMaster> Zones { get; set; } = new List<ZoneMaster>();
    }
}
