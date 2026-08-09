using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PGManagementSystem.Application.DTOs;
using PGManagementSystem.Application.Interfaces;
using PGManagementSystem.Application.Services;
using PGManagementSystem.Domain.Entities;

namespace PGManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/pg")]
    public class PGController : ControllerBase
    {
        private readonly PGService _pgService;

        public PGController(PGService pgService)
        {
            _pgService = pgService;
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreatePGDto dto)
        {
            var userId = int.Parse(User.FindFirst("userId").Value); // 🔥 JWT se

            await _pgService.CreatePG(dto, userId);

            return Ok("PG created successfully");
        }
    }
}
