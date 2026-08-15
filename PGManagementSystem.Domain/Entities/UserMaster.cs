using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PGManagementSystem.Domain.Entities;

[Table("USER_MST")]
public class UserMaster
{
    [Key]
    public Guid UserId { get; set; } = Guid.NewGuid(); // Unique User ID

    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Phone]
    [MaxLength(15)]
    public string Phone { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    // --- Foreign Key for Role ---
    [Required]
    public int RoleId { get; set; }

    [ForeignKey("RoleId")]
    public RoleMaster? Role { get; set; }
    [MaxLength(150)]
    public string? PgName { get; set; } // e.g., "Sunrise PG (Branch 1)"

    [MaxLength(255)]
    public string? Address { get; set; }

    [MaxLength(50)]
    public string? City { get; set; }

    // Hierarchy ke liye: Agar yeh Branch Admin hai, toh kis Super Admin ke under hai (Self-Referencing Foreign Key)
    public Guid? CreatedBySuperAdminId { get; set; }

    [ForeignKey("CreatedBySuperAdminId")]
    public UserMaster? SuperAdmin { get; set; }

    // Navigation Property: Ek Super Admin ya Admin ke under kitne sub-users (managers/staff) hain
    public ICollection<UserMaster>? SubUsers { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Property: PG Owner ke pass multiple Flats honge
    public ICollection<FlatMaster>? Flats { get; set; }
}