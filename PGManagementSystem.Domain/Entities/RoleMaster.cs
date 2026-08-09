using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGManagementSystem.Domain.Entities
{
    
        [Table("ROLE_MST")]
        public class RoleMaster
        {
            [Key]
            public int RoleId { get; set; } // Primary Key (1, 2, 3, etc.)

            [Required]
            [MaxLength(50)]
            public string RoleName { get; set; } = string.Empty; // "Admin", "PG_Owner", "Tenant"

            [MaxLength(150)]
            public string? Description { get; set; }

            public bool IsActive { get; set; } = true;
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

            // Navigation Property: Ek Role me multiple users ho sakte hain
            public ICollection<UserMaster>? Users { get; set; }
        }
    
}
