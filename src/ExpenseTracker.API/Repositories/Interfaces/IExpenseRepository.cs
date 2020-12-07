using ExpenseTracker.API.Dtos.Expenses;
using ExpenseTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Repositories.Interfaces
{
    public interface IExpenseRepository : IRepository<Expense>
    {
        Task<IEnumerable<ExpensesPerMonthDto>> GetExpensesForYearAsync(int userId, int year);
        Task<IEnumerable<ExpenseForSumDto>> GetSumForYear(int userId, int year);
        Task<IEnumerable<ExpenseForSumDto>> GetSumForMonth(int userId, int month);
        Task<IEnumerable<ExpenseForSumDto>> GetSumForDay(int userId, int day);
        Task<IEnumerable<PercentsTopicExpenseDto>> GetPercentsExpensesPerTopicAsync(int userId);
        Task<int> GetCountUserExpensesAsync(int userId);
        IEnumerable<ExpensesPerDaysDto> GetExpensesSumForCurrentMonth(int userId);
    }
}
