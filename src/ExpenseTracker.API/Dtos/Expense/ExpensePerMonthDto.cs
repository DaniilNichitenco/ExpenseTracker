using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Dtos.Expense
{
    public class ExpensePerMonthDto
    {
        public int Month { get; set; }
        public double Money { get; set; }
    }
}
