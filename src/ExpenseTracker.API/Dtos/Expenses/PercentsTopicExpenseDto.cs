﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Dtos.Expenses
{
    public class PercentsTopicExpenseDto
    {
        public string CurrencyCode { get; set; }
        public ICollection<SumExpensesPerTopicDto> Percents { get; set; }
    }
}
