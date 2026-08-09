namespace PGManagementSystem.Application.DTOs
{
    public class UpdateUserDto
    {

        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public int RoleId { get; set; }
    }
}
