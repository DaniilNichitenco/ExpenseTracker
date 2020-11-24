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

        public IEnumerable<ExpensesPerMonthDto> GetExpensesForYear(int userId, int year)
        {
            var month = DateTime.Now.Month;

            var allExpenses =  _context.Set<Expense>()
                .Where(e => e.OwnerId == userId && e.Date.Year == year)
                .AsEnumerable()
                .GroupBy(e => e.PurseId)
                .ToDictionary(g => g.Select(e => e.Purse.CurrencyCode).FirstOrDefault(), g => g.GroupBy(grp => grp.Date.Month)
                .Select(e => new ExpensePerMonthDto { Month = e.Key, Money = e.Sum(ex => ex.Money) }).ToList());

            var expenses = allExpenses.Select(e => new ExpensesPerMonthDto() { CurrencyCode = e.Key, Expenses = e.Value });

            foreach (var ex in expenses)
            {
                for (int i = 1; i <= month; i++)
                {
                    if (!ex.Expenses.Any(e => e.Month == i))
                    {
                        ex.Expenses.Insert(i - 1, new ExpensePerMonthDto() { Money = 0, Month = i });
                    }
                }
            }

            return expenses;
        }
    }
}
