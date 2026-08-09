using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PGManagementSystem.Application.DTOs;
using PGManagementSystem.Application.DTOs.Dashboard;
using PGManagementSystem.Application.Services;

namespace PGManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize] // Production mein token validation ke liye enable karein
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("summary")]
        public async Task<ActionResult<DashboardResponseDto>> GetDashboardSummary(
            [FromQuery] Guid userId,
            [FromQuery] string role,
            [FromQuery] int? month = null,
            [FromQuery] int? year = null)
        {
            try
            {
                if (userId == Guid.Empty || string.IsNullOrEmpty(role))
                {
                    return BadRequest(new { message = "UserId and Role are required parameters." });
                }

                var result = await _dashboardService.GetDashboardSummaryAsync(userId, role, month, year);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching dashboard data.", error = ex.Message });
            }
        }
    }
}