using System.ComponentModel.DataAnnotations;

namespace PGManagementSystem.Application.DTOs
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Invalid 10-digit Indian Mobile Number")]
        public string Mobile { get; set; } = string.Empty;

        [Required(ErrorMessage = "RoleId is required")]
        public int RoleId { get; set; } // Sirf RoleId lenge, DB validation ke liye
    }
}