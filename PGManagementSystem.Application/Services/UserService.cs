using Microsoft.Extensions.Logging;
using PGManagementSystem.Application.DTOs;
using PGManagementSystem.Application.Interfaces;
using PGManagementSystem.Domain.Entities;

namespace PGManagementSystem.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _repo;
        private readonly AuthService _jwt;
        private readonly ILogger<UserService> _logger;
        public UserService(IUserRepository repo, AuthService jwt, ILogger<UserService> logger)
        {
            _repo = repo;
            _jwt = jwt;
            _logger = logger;
        }

        public async Task RegisterUser(CreateUserDto userDto)
        {
            _logger.LogInformation("Registering user with Email: {Email} and Mobile: {Mobile}", userDto.Email, userDto.Mobile);

            // 1. Duplicate Email Check
            if (await _repo.IsEmailExistsAsync(userDto.Email))
            {
                throw new InvalidOperationException("Email address is already registered.");
            }

            // 2. Duplicate Mobile Check
            if (await _repo.IsPhoneExistsAsync(userDto.Mobile))
            {
                throw new InvalidOperationException("Mobile number is already registered.");
            }

            // 3. Hash Password & Map Entity
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            var user = new UserMaster
            {
                UserId = Guid.NewGuid(),
                FullName = userDto.Name,
                Email = userDto.Email.ToLower().Trim(),
                Phone = userDto.Mobile.Trim(), // Phone column me mobile store karenge
                PasswordHash = hashedPassword,
                RoleId = userDto.RoleId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddUser(user);
        }

        // --- 2. USER LOGIN ---
        public async Task<string> LoginUser(LoginDto loginDto)
        {
            _logger.LogInformation("Login attempt for email: {Name}", loginDto.Name);

            var user = await _repo.GetByName(loginDto.Name);
            if (user == null)
            {
                _logger.LogWarning("Invalid login attempt. User not found: {Email}", loginDto.Name);
                throw new KeyNotFoundException("Invalid email or password.");
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
            if (!isValidPassword)
            {
                _logger.LogWarning("Invalid password attempt for email: {Email}", loginDto.Name);
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            if (!user.IsActive)
            {
                throw new InvalidOperationException("Your account is deactivated. Please contact administrator.");
            }

            return _jwt.GenerateToken(user);
        }

        // --- 3. GET ALL USERS ---
        public async Task<List<UserResponseDto>> GetAllUsers()
        {
            var users = await _repo.GetAllUsers();

            return users.Select(u => new UserResponseDto
            {
                UserId = u.UserId,
                Name = u.FullName,
                Email = u.Email,
                RoleId = u.RoleId,
                Mobile = u.Phone,
                // Null checks for Role navigation property
                RoleName = u.Role != null ? u.Role.RoleName : "N/A",
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt,

                PgName = u.PgName,       
                Address = u.Address,
                City = u.City
            }).ToList();
        }

        // --- 4. UPDATE USER ---
        public async Task UpdateUser(Guid id, UpdateUserDto dto)
        {
            var user = await _repo.GetById(id);
            if (user == null)
            {
                _logger.LogWarning("Update failed. User ID {UserId} not found.", id);
                throw new KeyNotFoundException("User not found.");
            }

            user.FullName = dto.Name;
            user.Email = dto.Email;
            user.RoleId = dto.RoleId;
            user.IsActive = dto.IsActive;

            await _repo.UpdateUser(user);
            _logger.LogInformation("User ID {UserId} updated successfully.", id);
        }

        // --- 5. DELETE USER ---
        public async Task DeleteUser(Guid id)
        {
            var user = await _repo.GetById(id);
            if (user == null)
            {
                _logger.LogWarning("Delete failed. User ID {UserId} not found.", id);
                throw new KeyNotFoundException("User not found.");
            }

            // Soft Delete Option (Recommended for enterprise apps):
            // user.IsActive = false;
            // await _repo.UpdateUser(user);

            // Hard Delete:
            await _repo.DeleteUser(user);
            _logger.LogInformation("User ID {UserId} deleted successfully.", id);
        }
        public async Task<bool> UpdateProfileAsync(UpdateProfileDto dto)
        {
            // 1. Check if user exists
            var user = await _repo.GetById(dto.UserId);
            if (user == null)
            {
                return false; // User not found
            }

            // 2. Check Email Duplication for another user
            if (await _repo.IsEmailExistsForOtherUserAsync(dto.Email, dto.UserId))
            {
                throw new InvalidOperationException("This Email is already registered with another account.");
            }

            // 3. Check Phone Duplication for another user (Since login uses phone)
            if (await _repo.IsPhoneExistsForOtherUserAsync(dto.Phone, dto.UserId))
            {
                throw new InvalidOperationException("This Phone Number is already registered with another account.");
            }

            // 4. Update allowed specific profile fields only
            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.Phone = dto.Phone;
            user.PgName = dto.PgName;
            user.Address = dto.Address;
            user.City = dto.City;

            // 5. Save via Repository
            return await _repo.UpdateUserProfileAsync(user);
        }
    }


}
