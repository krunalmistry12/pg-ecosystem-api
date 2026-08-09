using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PGManagementSystem.Application.DTOs.Rent;
using PGManagementSystem.Application.Interfaces;

namespace PGManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentController : ControllerBase
    {
        private readonly IRentService _rentService;

        public RentController(IRentService rentService)
        {
            _rentService = rentService;
        }

        // =========================================================================
        // 👑 ADMIN / PG OWNER ENDPOINTS
        // =========================================================================

        /// <summary>
        /// PG Owner: Generate Rent Bill for a Tenant
        /// </summary>
        [HttpPost("generate-bill")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GenerateRentBill([FromBody] GenerateRentBillDto dto)
        {
            try
            {
                var response = await _rentService.GenerateRentBillAsync(dto);
                return Ok(new { success = true, message = "Rent bill generated successfully.", data = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// PG Owner: Record Payment from a Tenant
        /// </summary>
        [HttpPost("record-payment")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RecordPayment([FromBody] RecordPaymentDto dto)
        {
            try
            {
                var receipt = await _rentService.RecordPaymentAsync(dto);
                return Ok(new { success = true, message = "Payment recorded successfully.", data = receipt });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// PG Owner: Get All Rent Records across all flats owned by logged-in PG Owner
        /// </summary>
        [HttpGet("admin/all-records")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllRentRecordsForAdmin(
            [FromQuery] int? month,
            [FromQuery] int? year,
            [FromQuery] string? status,
            [FromQuery] string? search)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid ownerId))
                {
                    return Unauthorized(new { success = false, message = "Invalid owner credentials." });
                }

                var records = await _rentService.GetAllRentRecordsForAdminAsync(ownerId, month, year, status, search);
                return Ok(new { success = true, totalRecords = records.Count, data = records });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // =========================================================================
        // 🧑‍🎓 TENANT ENDPOINTS
        // =========================================================================

        /// <summary>
        /// Tenant: View All My Rent Bills (Paid, Pending, Partial)
        /// </summary>
        [HttpGet("tenant/my-bills")]
        [Authorize(Roles = "Tenant")]
        public async Task<IActionResult> GetMyRentBills()
        {
            try
            {
                // JWT Token se Logged-in Tenant ki TenantId extract kar rahe hain
                var tenantIdClaim = User.FindFirst("TenantId")?.Value
                                 ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(tenantIdClaim) || !long.TryParse(tenantIdClaim, out long tenantId))
                {
                    return Unauthorized(new { success = false, message = "Invalid tenant token." });
                }

                var bills = await _rentService.GetAllRentsByTenantIdAsync(tenantId);
                return Ok(new { success = true, totalRecords = bills.Count, data = bills });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Tenant: View Only My Pending Bills
        /// </summary>
        [HttpGet("tenant/my-pending-bills")]
        [Authorize(Roles = "Tenant")]
        public async Task<IActionResult> GetMyPendingBills()
        {
            try
            {
                var tenantIdClaim = User.FindFirst("TenantId")?.Value
                                 ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(tenantIdClaim) || !long.TryParse(tenantIdClaim, out long tenantId))
                {
                    return Unauthorized(new { success = false, message = "Invalid tenant token." });
                }

                var bills = await _rentService.GetPendingBillsByTenantIdAsync(tenantId);
                return Ok(new { success = true, totalRecords = bills.Count, data = bills });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Tenant: View My Payment History Receipts
        /// </summary>
        [HttpGet("tenant/my-payment-history")]
        [Authorize(Roles = "Tenant")]
        public async Task<IActionResult> GetMyPaymentHistory()
        {
            try
            {
                var tenantIdClaim = User.FindFirst("TenantId")?.Value
                                 ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(tenantIdClaim) || !long.TryParse(tenantIdClaim, out long tenantId))
                {
                    return Unauthorized(new { success = false, message = "Invalid tenant token." });
                }

                var history = await _rentService.GetPaymentHistoryByTenantIdAsync(tenantId);
                return Ok(new { success = true, totalRecords = history.Count, data = history });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}