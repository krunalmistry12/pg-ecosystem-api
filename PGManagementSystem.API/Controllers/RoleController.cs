using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PGManagementSystem.Application.Services;

namespace PGManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(await _roleService.GetAllRoles());
        }

        [HttpPost]
        public async Task<IActionResult> Create(string roleName)
        {
            var role = await _roleService.CreateRole(roleName);
            return Ok(role);
        }
    }
}
