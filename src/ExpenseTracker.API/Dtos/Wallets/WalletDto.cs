using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Dtos.Wallets
{
    public class WalletDto
    {
        public int Id { get; set; }

        private double _bill;
        public double Bill
        {
            get
            {
                return _bill;
            }
            set
            {
                if (value < 0d)
                {
                    throw new ArgumentOutOfRangeException("value", "value of bill must be greater then 0!");
                }
                _bill = value;
            }
        }
        public string CurrencyCode { get; set; }
    }
}
