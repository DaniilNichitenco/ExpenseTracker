using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Dtos.Expenses
{
    public class ExpensesPerDaysDto
    {
        public string CurrencyCode { get; set; }
        public List<ExpensePerDayDto> ExpensesPerDay { get; set; } 
    }
}
