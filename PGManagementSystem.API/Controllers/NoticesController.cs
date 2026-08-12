using Microsoft.AspNetCore.Mvc;
using PGManagementSystem.Application.DTOs;
using PGManagementSystem.Application.DTOs.Notice;
using PGManagementSystem.Application.Interfaces;
using System.Threading.Tasks;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
public class NoticesController : ControllerBase
{
    private readonly INoticeService _noticeService;

    public NoticesController(INoticeService noticeService)
    {
        _noticeService = noticeService;
    }

    // POST: api/notices
    [HttpPost]
    public async Task<IActionResult> CreateNotice([FromBody] CreateNoticeDto model)
    {
        if (string.IsNullOrEmpty(model.Title) || string.IsNullOrEmpty(model.Description))
        {
            return BadRequest(new { success = false, message = "Title and Description are required." });
        }

        var result = await _noticeService.CreateNoticeAsync(model);
        return Ok(new { success = true, message = "Notice published successfully!", data = result });
    }

    // GET: api/notices?flatid=...
    [HttpGet]
    public async Task<IActionResult> GetNotices([FromQuery] Guid? flatid)
    {
        var result = await _noticeService.GetNoticesAsync(flatid);
        return Ok(new { success = true, data = result });
    }

    // GET: api/notices/admin?flatid=...
    [HttpGet("admin")]
    [Authorize]
    public async Task<IActionResult> GetNoticesByAdmin()
    {
        var adminIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(adminIdClaim))
        {
            return Unauthorized(new { success = false, message = "Admin ID not found in token" });
        }

        if (!Guid.TryParse(adminIdClaim, out Guid adminGuid))
        {
            return BadRequest(new { success = false, message = "Invalid Admin ID format in token" });
        }

        var result = await _noticeService.GetNoticesByAdminAsync(adminGuid);

        return Ok(new { success = true, data = result });
    }
}