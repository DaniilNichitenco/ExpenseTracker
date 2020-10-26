using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Domain.Purses
{
    public abstract class Purse : BaseEntity
    {
        public int PersonId { get; set; }
        public Person Person { get; set; }
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
    }
}
