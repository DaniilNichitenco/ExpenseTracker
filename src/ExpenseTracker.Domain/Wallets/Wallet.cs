using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Domain.Wallets
{
    public abstract class Wallet : BaseEntity
    {
        double _bill;
        public double Bill
        {
            get
            {
               return _bill;
            }
            set
            {
                if(value < 0d)
                {
                    throw new ArgumentOutOfRangeException("value", "value of bill must be greater then 0!");
                }
                _bill = value;
            }
        }
        public string CurrencyCode { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
