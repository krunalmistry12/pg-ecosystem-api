using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGManagementSystem.Application.Interfaces;
using PGManagementSystem.Domain.Entities;

namespace PGManagementSystem.Application.Services
{
    public class RoleService 
    {
        private readonly IRoleService _repo;
        private readonly AuthService _jwt;
        public RoleService(IRoleService repo, AuthService jwt)
        {
            _repo = repo;
            _jwt = jwt;
        }
        public async Task<List<RoleMaster>> GetAllRoles()
        {
            return await _repo.GetAllRoles();
        }

        public async Task<RoleMaster> CreateRole(string roleName)
        {
            var role = new RoleMaster { RoleName = roleName };

            await _repo.CreateRole(roleName);
            return role;
        }
    }
}
