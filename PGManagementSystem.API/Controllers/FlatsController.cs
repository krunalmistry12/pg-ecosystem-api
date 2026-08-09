using Microsoft.AspNetCore.Mvc;
using PGManagementSystem.Application.DTOs;
using PGManagementSystem.Application.Interfaces;

namespace PGManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlatsController : ControllerBase
    {
        private readonly IFlatService _flatService;

        public FlatsController(IFlatService flatService)
        {
            _flatService = flatService;
        }

        /// <summary>
        /// Dashboard ke liye saare Flats ka summary summary list fetch karta hai
        /// GET: api/Flats/user/{userId}
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetFlatCards(Guid userId)
        {
            var flats = await _flatService.GetFlatCardsByUserIdAsync(userId);
            return Ok(new { success = true, data = flats });
        }

        /// <summary>
        /// Single Flat ki poori detail fetch karta hai (React Native Edit Screen ke liye)
        /// GET: api/Flats/{id} -> Returns FlatDetailDto
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFlatById(Guid id)
        {
            var flat = await _flatService.GetFlatByIdAsync(id);
            if (flat == null)
                return NotFound(new { success = false, message = "Flat nahi mila." });

            return Ok(new { success = true, data = flat });
        }

        /// <summary>
        /// Naya Flat create karne ke liye
        /// POST: api/Flats
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateFlat([FromBody] CreateFlatDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, errors = ModelState });

            try
            {
                await _flatService.CreateFlatAsync(dto);
                return Ok(new { success = true, message = "Flat successfully create ho gaya!" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Server error while creating flat.", details = ex.Message });
            }
        }

        /// <summary>
        /// Existing Flat, Zones aur Beds update karne ke liye
        /// PUT: api/Flats/{id}
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFlat(Guid id, [FromBody] CreateFlatDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, errors = ModelState });

            var updated = await _flatService.UpdateFlatAsync(id, dto);
            if (!updated)
                return NotFound(new { success = false, message = "Flat update karne ke liye nahi mila." });

            return Ok(new { success = true, message = "Flat details successfully update ho gayi hain." });
        }

        /// <summary>
        /// Flat delete karne ke liye
        /// DELETE: api/Flats/{id}
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlat(Guid id)
        {
            var (success, message) = await _flatService.DeleteFlatAsync(id);
            if (!success)
                return BadRequest(new { success = false, message });

            return Ok(new { success = true, message });
        }
    }
}