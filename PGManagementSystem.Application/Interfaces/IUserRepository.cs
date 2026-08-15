using PGManagementSystem.Domain.Entities;

namespace PGManagementSystem.Application.Interfaces;

public interface IUserRepository
{
    Task AddUser(UserMaster user);
    Task UpdateUser(UserMaster user);
    Task DeleteUser(UserMaster user);
    Task<UserMaster> VerifyUser(string Name);
    Task<List<UserMaster>> GetAllUsers();
    Task<UserMaster> GetById(Guid id);
    Task<UserMaster?> GetByName(string email);
    Task<bool> IsEmailExistsAsync(string email);
    Task<bool> IsPhoneExistsAsync(string phone);

    Task<bool> IsEmailExistsForOtherUserAsync(string email, Guid currentUserId);
    Task<bool> IsPhoneExistsForOtherUserAsync(string phone, Guid currentUserId);
    Task<bool> UpdateUserProfileAsync(UserMaster user); // Repository level profile update
}