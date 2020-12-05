using AutoMapper;
using ExpenseTracker.API.Dtos.Expenses;
using ExpenseTracker.API.Repositories.Interfaces;
using ExpenseTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.API.Repositories.Implementations
{
    public class ExpenseRepository : Repository<Expense>, IExpenseRepository
    {
        public ExpenseRepository(ExpenseTrackerDbContext context, IMapper mapper) : base(context, mapper)
        {

        }

        public async Task<int> GetCountUserExpensesAsync(int userId)
        {
            return await _context.Set<Expense>().Where(e => e.OwnerId == userId).CountAsync();
        }

        public async Task<IEnumerable<ExpensesPerMonthDto>> GetExpensesForYearAsync(int userId, int year)
        {
            var month = DateTime.Now.Month;

            var allExpenses =  _context.Set<Expense>()
                .Where(e => e.OwnerId == userId && e.Date.Year == year)
                .AsEnumerable()
                .GroupBy(e => e.PurseId)
                .ToDictionary(g => g.FirstOrDefault().Purse.CurrencyCode, g => g.GroupBy(grp => grp.Date.Month)
                .Select(e => new ExpensePerMonthDto { Month = e.Key, Money = e.Sum(ex => ex.Money) }).ToList());

            var expenses = allExpenses.Select(e => new ExpensesPerMonthDto() { CurrencyCode = e.Key, Expenses = e.Value });

            foreach (var ex in expenses)
            {
                for (int i = 1; i <= month; i++)
                {
                    if (!ex.Expenses.Any(e => e.Month == i))
                    {
                        ex.Expenses.Add(new ExpensePerMonthDto() { Money = 0, Month = i });
                    }
                }

                ex.Expenses.Sort((f, s) => f.Month.CompareTo(s.Month));
            }

            return expenses;
        }

        public async Task<IEnumerable<ExpenseForSumDto>> GetSumForYear(int userId, int year)
        {
            var expenses = await Where(e => e.OwnerId == userId && e.Date.Year == year);
            var groupSum = expenses.GroupBy(e => e.PurseId)
                .Select(g => new ExpenseForSumDto() { CurrencyCode = g.Select(e => e.Purse.CurrencyCode)
                    .FirstOrDefault(), Sum = g.Sum(e => e.Money) });

            return groupSum;
        }

        public async Task<IEnumerable<ExpenseForSumDto>> GetSumForMonth(int userId, int month)
        {
            var year = DateTime.Now.Year;
            var expenses = await Where(e => e.OwnerId == userId && e.Date.Year == year && e.Date.Month == month);
            var groupSum = expenses.GroupBy(e => e.PurseId)
                .Select(g => new ExpenseForSumDto() { CurrencyCode = g.Select(e => e.Purse.CurrencyCode)
                    .FirstOrDefault(), Sum = g.Sum(e => e.Money) });

            return groupSum;
        }

        public async Task<IEnumerable<ExpenseForSumDto>> GetSumForDay(int userId, int day)
        {
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            var expenses = await Where(e => e.OwnerId == userId 
                && e.Date.Year == year && e.Date.Month == month && e.Date.Day == day);
            var groupSum = expenses.GroupBy(e => e.PurseId)
                .Select(g => new ExpenseForSumDto()
                {
                    CurrencyCode = g.Select(e => e.Purse.CurrencyCode)
                    .FirstOrDefault(),
                    Sum = g.Sum(e => e.Money)
                });

            return groupSum;
        }

        public async Task<IEnumerable<PercentsTopicExpense>> GetPercentsExpensesPerTopicAsync(int userId)
        {
            var allExpenses = _context.Set<Expense>()
                .Where(e => e.OwnerId == userId)
                .AsEnumerable()
                .GroupBy(e => e.PurseId)
                .ToDictionary(g => g.FirstOrDefault().Purse.CurrencyCode, g => g.GroupBy(grp => grp.TopicId)
                .Select(e => new SumExpensesPerTopicDto { Topic = e.FirstOrDefault().Topic.Name, Sum = e.Sum(ex => ex.Money) }).ToList());

            var topics = _context.Set<Topic>().Where(t => t.OwnerId == userId);
            var names = await topics.Select(t => t.Name).ToListAsync();

            foreach (var purse in allExpenses)
            {
                double sum = 0;
                sum = purse.Value.Sum(e => e.Sum);

                if (sum == 0)
                {
                    sum = 1; //Avoid DivideByZeroException
                }

                foreach (var topic in purse.Value)
                {
                    topic.Sum = topic.Sum / sum * 100;
                }
            }

            foreach(var purse in allExpenses)
            {
                for (int i = 0; i < names.Count(); i++)
                {
                    if (!purse.Value.Any(p => p.Topic == names[i]))
                    {
                        purse.Value.Insert(i, new SumExpensesPerTopicDto() { Topic = names[i], Sum = 0 });
                    }
                }

                purse.Value.Sort((first, second) => first.Topic.CompareTo(second.Topic));
            }

            var percents = allExpenses.Select(e => new PercentsTopicExpense() { CurrencyCode = e.Key, Percents = e.Value });

            return percents;
        }
    }
}
