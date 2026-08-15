using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PGManagementSystem.Application.DTOs.Complaint;
using PGManagementSystem.Application.Services;
using System.Security.Claims; // ClaimTypes ke liye zaroori hai

namespace PGManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintsController : ControllerBase
    {
        private readonly ComplaintService _complaintService;

        public ComplaintsController(ComplaintService complaintService)
        {
            _complaintService = complaintService;
        }

        // =====================================================================
        // TENANT ENDPOINTS
        // =====================================================================

        [HttpPost("tenant/create")]
        [Authorize(Roles = "Tenant")]
        public async Task<IActionResult> CreateComplaint([FromBody] CreateComplaintDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, errors = ModelState });
            }

            var createdComplaint = await _complaintService.CreateComplaintAsync(dto);
            return Ok(new { success = true, message = "Complaint raised successfully.", data = createdComplaint });
        }

        /// <summary>
        /// Tenant can view their own complaints status and admin remarks
        /// </summary>
        [HttpGet("tenant/my-complaints")]
        [Authorize(Roles = "Tenant")]
        public async Task<IActionResult> GetMyComplaints()
        {
            // JWT Token se logged-in user ki ID nikalna
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int tenantId))
            {
                return Unauthorized(new { success = false, message = "Invalid token or User ID not found." });
            }

            var complaints = await _complaintService.GetComplaintsByTenantIdAsync(tenantId);
            return Ok(new { success = true, data = complaints });
        }

        // =====================================================================
        // ADMIN / MANAGER ENDPOINTS
        // =====================================================================

        [HttpGet("admin/all")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetAllComplaints()
        {
            var complaints = await _complaintService.GetAllComplaintsAsync();
            return Ok(new { success = true, data = complaints });
        }

        [HttpPut("admin/update-status")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateComplaint([FromBody] UpdateComplaintDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, errors = ModelState });
            }
            var adminIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(adminIdClaim) || !Guid.TryParse(adminIdClaim, out Guid adminId))
            {
                return Unauthorized(new { success = false, message = "Invalid token or Admin ID not found." });
            }

            var updatedComplaint = await _complaintService.UpdateComplaintStatusAsync(dto, adminId);
            if (updatedComplaint == null)
            {
                return NotFound(new { success = false, message = "Complaint record not found." });
            }

            return Ok(new { success = true, message = "Complaint updated successfully.", data = updatedComplaint });
        }
    }
}