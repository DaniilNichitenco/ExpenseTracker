using ExpenseTracker.Domain.Purses;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Domain
{
    public class Expense : BaseEntity
    {
        public int PurseId { get; set; }
        public virtual Purse Purse { get; set; }
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
                if(value < 0d)
                {
                    throw new ArgumentOutOfRangeException("value", "value of money must be greater then 0!");
                }
                _money = value;
            }
        }
        private double _money;
    }
}
