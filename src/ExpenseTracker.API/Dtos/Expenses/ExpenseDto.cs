﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Dtos.Expenses
{
    public class ExpenseDto
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public double Money
        {
            get
            {
                return _money;
            }
            set
            {
                if (value < 0d)
                {
                    throw new ArgumentOutOfRangeException("value", "value of money must be greater then 0!");
                }
                _money = value;
            }
        }
        private double _money;
    }
}
