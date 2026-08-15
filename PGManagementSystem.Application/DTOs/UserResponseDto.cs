namespace PGManagementSystem.Application.DTOs
{
    public class UserResponseDto
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string Mobile { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty; // Response me RoleName bhej sakte hain (e.g. "Admin", "Tenant")
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? PgName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}