using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PGManagementSystem.Application.DTOs.Expense;
using PGManagementSystem.Application.Services;

namespace PGManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Manager")] // Pure controller par Admin/Manager lock lag gaya
    public class ExpensesController : ControllerBase
    {
        private readonly ExpenseService _expenseService;

        public ExpensesController(ExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        /// <summary>
        /// Get all expenses (Common & Flat-wise expenses of PG)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllExpenses()
        {
            var expenses = await _expenseService.GetAllExpensesAsync();
            return Ok(new { success = true, data = expenses });
        }

        /// <summary>
        /// Add a new business expense (e.g., Light bill, Salary, Software)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, errors = ModelState });
            }

            try
            {
                var createdExpense = await _expenseService.CreateExpenseAsync(dto);
                return Ok(new { success = true, message = "Expense added successfully.", data = createdExpense });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Delete an expense record
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(Guid id)
        {
            var deleted = await _expenseService.DeleteExpenseAsync(id);
            if (!deleted)
            {
                return NotFound(new { success = false, message = "Expense record not found." });
            }

            return Ok(new { success = true, message = "Expense deleted successfully." });
        }
    }
}
