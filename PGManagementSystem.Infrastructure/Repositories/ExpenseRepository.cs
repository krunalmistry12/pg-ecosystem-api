using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PGManagementSystem.Application.Interfaces;
using PGManagementSystem.Domain.Entities;
using PGManagementSystem.Infrastructure.Data;

namespace PGManagementSystem.Infrastructure.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly AppDbContext _context;

        public ExpenseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ExpenseMaster>> GetAllAsync()
        {
            return await _context.Expenses
                .Include(e => e.Flat)
                .Include(e => e.CreatedByUser)
                .ToListAsync();
        }

        public async Task<IEnumerable<ExpenseMaster>> GetByFlatIdAsync(Guid flatId)
        {
            return await _context.Expenses
                .Include(e => e.Flat)
                .Include(e => e.CreatedByUser)
                .Where(e => e.FlatId == flatId || e.IsCommonExpense == true) 
                .ToListAsync();
        }

        public async Task<IEnumerable<ExpenseMaster>> GetCommonExpensesAsync()
        {
            return await _context.Expenses
                .Include(e => e.CreatedByUser)
                .Where(e => e.IsCommonExpense == true)
                .ToListAsync();
        }

        public async Task<ExpenseMaster?> GetByIdAsync(Guid expenseId)
        {
            return await _context.Expenses
                .Include(e => e.Flat)
                .Include(e => e.CreatedByUser)
                .FirstOrDefaultAsync(e => e.ExpenseId == expenseId);
        }

        public async Task AddAsync(ExpenseMaster expense)
        {
            await _context.Expenses.AddAsync(expense);
        }

        public async Task DeleteAsync(ExpenseMaster expense)
        {
            _context.Expenses.Remove(expense);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
