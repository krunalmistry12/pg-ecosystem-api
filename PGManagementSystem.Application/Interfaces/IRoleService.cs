using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGManagementSystem.Domain.Entities;

namespace PGManagementSystem.Application.Services
{
    public interface IRoleService
    {
        Task<List<RoleMaster>> GetAllRoles();
        Task<RoleMaster> CreateRole(string roleName);
    }
}
