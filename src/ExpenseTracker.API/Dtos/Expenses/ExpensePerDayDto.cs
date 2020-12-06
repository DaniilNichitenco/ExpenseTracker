using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Dtos.Expenses
{
    public class ExpensePerDayDto
    {
        public double Sum { get; set; }
        public int Day { get; set; }
    }
}
