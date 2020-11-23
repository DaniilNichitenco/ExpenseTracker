using AutoMapper;
using ExpenseTracker.API.Dtos.Expense;
using ExpenseTracker.API.Repositories.Interfaces;
using ExpenseTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ExpenseTracker.API.Repositories.Implementations
{
    public class ExpenseRepository : Repository<Expense>, IExpenseRepository
    {
        public ExpenseRepository(ExpenseTrackerDbContext context, IMapper mapper) : base(context, mapper)
        {

        }

        public async Task<IDictionary<string, IEnumerable<ExpensesPerMonthDto>>> GetMonthlyExpenses(int userId, int year)
        {
            var allExpenses = await _context.Set<Expense>()
                .Where(e => e.OwnerId == userId && e.Date.Year == year)
                .GroupBy(e => e.PurseId)
                .ToDictionaryAsync(g => g.Key.ToString(), g => g.GroupBy(grp => grp.Date.Month)
                .Select(e => new ExpensesPerMonthDto { Month = e.Key.ToString(), Money = e.Sum(ex => ex.Money) }));


            return allExpenses;
        }
    }
}
