using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGManagementSystem.Domain.Entities;

namespace PGManagementSystem.Application.Interfaces
{
    public interface IExpenseRepository
    {
        Task<IEnumerable<ExpenseMaster>> GetAllAsync();
        Task<IEnumerable<ExpenseMaster>> GetByFlatIdAsync(Guid flatId);
        Task<IEnumerable<ExpenseMaster>> GetCommonExpensesAsync();
        Task<ExpenseMaster?> GetByIdAsync(Guid expenseId);
        Task AddAsync(ExpenseMaster expense);
        Task DeleteAsync(ExpenseMaster expense);
        Task SaveChangesAsync();
    }
}
