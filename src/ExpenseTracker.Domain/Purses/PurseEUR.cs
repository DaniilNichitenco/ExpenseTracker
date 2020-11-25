using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Domain.Purses
{
    public class PurseEUR : Purse
    {
        public PurseEUR()
        {
            CurrencyCode = "eur";
        }
    }
}
