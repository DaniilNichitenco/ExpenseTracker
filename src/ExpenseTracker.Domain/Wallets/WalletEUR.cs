using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Domain.Wallets
{
    public class WalletEUR : Wallet
    {
        public WalletEUR()
        {
            CurrencyCode = "eur";
        }
    }
}
