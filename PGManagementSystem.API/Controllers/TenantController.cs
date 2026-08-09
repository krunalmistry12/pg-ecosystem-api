using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PGManagementSystem.Application.DTOs;
using PGManagementSystem.Application.Interfaces;
using PGManagementSystem.Domain.Enums;

namespace PGManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantsController : ControllerBase
    {
        private readonly ITenantService _tenantService;

        public TenantsController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        // 1. POST: Create New Tenant (Fast Creation)
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateTenant([FromForm] CreateTenantDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tenant = await _tenantService.AddTenantAsync(dto);
            return CreatedAtAction(nameof(GetTenantById), new { id = tenant.Id }, tenant);
        }

        // 2. PUT: Update Tenant Details / Room Switch / Edit Profile (NEW)
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateTenant(long id, [FromForm] UpdateTenantDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedTenant = await _tenantService.UpdateTenantAsync(id, dto);
            if (updatedTenant == null)
            {
                return NotFound(new { success = false, message = $"Tenant with ID {id} not found." });
            }

            return Ok(new
            {
                success = true,
                message = "Tenant details updated successfully!",
                data = updatedTenant
            });
        }

        // 3. GET: Get Tenant By ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTenantById(long id)
        {
            var tenantDto = await _tenantService.GetTenantByIdAsync(id);

            if (tenantDto == null)
            {
                return NotFound(new { success = false, message = $"Tenant with ID {id} not found." });
            }

            return Ok(new
            {
                success = true,
                data = tenantDto
            });
        }

        // 4. PATCH: Update Tenant Status (ACTIVE / INACTIVE)
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateTenantStatus(long id, [FromBody] enumTenantStatus status)
        {
            var result = await _tenantService.ChangeTenantStatusAsync(id, status);
            if (!result) return NotFound("Tenant not found");

            return Ok(new { message = $"Tenant status updated to {status}" });
        }

        // 5. GET: Get All Tenants By User ID (Owner Dashboard)
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetTenantsByUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { success = false, message = "User ID is required" });
            }

            try
            {
                var tenants = await _tenantService.GetTenantsByUserIdAsync(userId);

                return Ok(new
                {
                    success = true,
                    data = tenants
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Server error", details = ex.Message });
            }
        }
    }
}