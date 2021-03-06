﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Dtos.Expenses
{
    public class ExpensesPerMonthDto
    {
        public string CurrencyCode { get; set; }
        public List<ExpensePerMonthDto> Expenses { get; set; }
    }
}
