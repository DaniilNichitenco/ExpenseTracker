using ExpenseTracker.API.Dtos.Expense;
using ExpenseTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Repositories.Interfaces
{
    public interface IExpenseRepository : IRepository<Expense>
    {
        IEnumerable<ExpensesPerMonthDto> GetExpensesForYear(int userId, int year);
    }
}
