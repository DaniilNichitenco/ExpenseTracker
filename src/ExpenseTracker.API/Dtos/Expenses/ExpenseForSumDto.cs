using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Dtos.Expenses
{
    public class ExpenseForSumDto
    {
        public string CurrencyCode { get; set; }
        public double Sum { get; set; }
    }
}
