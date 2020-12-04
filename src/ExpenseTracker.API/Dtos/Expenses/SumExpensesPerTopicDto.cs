using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Dtos.Expenses
{
    public class SumExpensesPerTopicDto
    {
        public string Topic { get; set; }
        public double Sum { get; set; }
    }
}
