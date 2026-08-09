using Microsoft.EntityFrameworkCore;
using PGManagementSystem.Application.Interfaces;
using PGManagementSystem.Application.Services;
using PGManagementSystem.Domain.Entities;
using PGManagementSystem.Infrastructure.Data;

namespace PGManagementSystem.Infrastructure.Repositories
{
    public class RoleRepository : IRoleService
    {
        private readonly AppDbContext _context;
        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<RoleMaster>> GetAllRoles()
        {
            return await _context.RoleMasters.ToListAsync();
        }

        public async Task<RoleMaster> CreateRole(string roleName)
        {
            var role = new RoleMaster { RoleName = roleName };
            _context.RoleMasters.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }
    }
}

