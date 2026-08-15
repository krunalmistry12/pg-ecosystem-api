using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGManagementSystem.Application.DTOs.Expense;
using PGManagementSystem.Application.Interfaces;
using PGManagementSystem.Domain.Entities;

namespace PGManagementSystem.Application.Services
{
    public class ExpenseService
    {
        private readonly IExpenseRepository _expenseRepo;

        public ExpenseService(IExpenseRepository expenseRepo)
        {
            _expenseRepo = expenseRepo;
        }

        public async Task<IEnumerable<ExpenseMaster>> GetAllExpensesAsync()
        {
            return await _expenseRepo.GetAllAsync();
        }

        public async Task<IEnumerable<ExpenseMaster>> GetTenantExpensesAsync(Guid flatId)
        {
            return await _expenseRepo.GetByFlatIdAsync(flatId);
        }

        public async Task<ExpenseMaster> CreateExpenseAsync(CreateExpenseDto dto)
        {
            // --- Validation Rules ---
            if (!dto.IsCommonExpense && dto.FlatId == null)
            {
                throw new ArgumentException("FlatId is required when the expense is not common.");
            }

            if (dto.IsCommonExpense && dto.FlatId != null)
            {
                // Optional: Agar common hai toh flatId ko forcefully null kar sakte hain
                dto.FlatId = null;
            }

            var expense = new ExpenseMaster
            {
                ExpenseId = Guid.NewGuid(),
                FlatId = dto.FlatId,
                IsCommonExpense = dto.IsCommonExpense,
                UserId = dto.UserId,
                Title = dto.Title,
                Category = dto.Category,
                Amount = dto.Amount,
                Month = dto.Month,
                Date = dto.Date,
                PaymentMode = dto.PaymentMode,
                PaidBy = dto.PaidBy,
                Status = dto.Status,
                ReceiptName = dto.ReceiptName,
                ReceiptUri = dto.ReceiptUri,
                Notes = dto.Notes,
                CreatedAt = DateTime.UtcNow
            };

            await _expenseRepo.AddAsync(expense);
            await _expenseRepo.SaveChangesAsync();

            return expense;
        }

        public async Task<bool> DeleteExpenseAsync(Guid expenseId)
        {
            var expense = await _expenseRepo.GetByIdAsync(expenseId);
            if (expense == null) return false;

            await _expenseRepo.DeleteAsync(expense);
            await _expenseRepo.SaveChangesAsync();
            return true;
        }
    }
}
