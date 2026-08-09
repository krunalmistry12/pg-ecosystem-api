using PGManagementSystem.Domain.Entities;
using PGManagementSystem.Infrastructure.Data;
using PGManagementSystem.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace PGManagementSystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddUser(UserMaster user)
        {
            await _context.UserMasters.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task<UserMaster> VerifyUser(string Name)
        {
            return await _context.UserMasters.Include(x => x.Role).FirstOrDefaultAsync(u => u.FullName == Name);
        }
        public async Task<List<UserMaster>> GetAllUsers()
        {
            return await _context.UserMasters
                .Include(u => u.Role)
                .ToListAsync();
        }
        public async Task<UserMaster> GetById(int id)
        {
            return await _context.UserMasters.FindAsync(id);
        }

        public async Task UpdateUser(UserMaster user)
        {
            _context.UserMasters.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteUser(UserMaster user)
        {
            _context.UserMasters.Remove(user);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _context.UserMasters.AnyAsync(u => u.Email.ToLower() == email.ToLower().Trim());
        }

        public async Task<bool> IsPhoneExistsAsync(string phone)
        {
            return await _context.UserMasters.AnyAsync(u => u.Phone.Trim() == phone.Trim());
        }
        public async Task<UserMaster?> GetById(Guid id)
        {
            return await _context.UserMasters
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<UserMaster?> GetByName(string FullName)
        {
            return await _context.UserMasters
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.FullName == FullName);
        }
    }
}
