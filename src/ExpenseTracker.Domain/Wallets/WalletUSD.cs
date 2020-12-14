using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Domain.Wallets
{
    public class WalletUSD : Wallet
    {
        public WalletUSD()
        {
            CurrencyCode = "usd";
        }
    }
}
