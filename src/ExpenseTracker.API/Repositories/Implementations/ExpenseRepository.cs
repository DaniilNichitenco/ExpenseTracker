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

        public IDictionary<string, List<ExpensesPerMonthDto>> GetExpensesForYear(int userId, int year)
        {
            var allExpenses =  _context.Set<Expense>()
                .Where(e => e.OwnerId == userId && e.Date.Year == year)
                .AsEnumerable()
                .GroupBy(e => e.PurseId)
                .ToDictionary(g => g.Key.ToString(), g => g.GroupBy(grp => grp.Date.Month)
                .Select(e => new ExpensesPerMonthDto { Month = e.Key, Money = e.Sum(ex => ex.Money) }).ToList());


            return allExpenses;
        }
    }
}
