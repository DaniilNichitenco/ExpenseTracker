using ExpenseTracker.Domain.Wallets;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Domain
{
    public class Expense : BaseEntity
    {
        public int WalletId { get; set; }
        public virtual Wallet Wallet { get; set; }
        public int? TopicId { get; set; }
        public virtual Topic Topic { get; set; }
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
