using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Domain.Wallets
{
    public class WalletMDL : Wallet
    {
        public WalletMDL()
        {
            CurrencyCode = "mdl";
        }
    }
}
