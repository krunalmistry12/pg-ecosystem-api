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

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Property: PG Owner ke pass multiple Flats honge
    public ICollection<FlatMaster>? Flats { get; set; }
}