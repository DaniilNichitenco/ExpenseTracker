using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Domain.Wallets
{
    public sealed class WalletFactory
    {
        public static Wallet CreateEmptyWallet(string currencyCode)
        {
            
            switch (currencyCode)
            {
                case "mdl":
                    return CreateWalletMDL(0);
                case "usd":
                    return CreateWalletUSD(0);
                case "eur":
                    return CreateWalletEUR(0);
                default:
                    throw new ArgumentException($"Cannot create {currencyCode} wallet");
            }
        }

        public static List<string> GetAllCurrencies()
        {
            var currencies = new List<string>();
            currencies.AddRange(new List<string>() 
            { 
                CreateWalletEUR(0).CurrencyCode,
                CreateWalletMDL(0).CurrencyCode,
                CreateWalletUSD(0).CurrencyCode
            });

            return currencies;
        }

        private static WalletMDL CreateWalletMDL(double bill)
        {
            return new WalletMDL() { Bill = bill };
        }

        private static WalletUSD CreateWalletUSD(double bill)
        {
            return new WalletUSD() { Bill = bill };
        }

        private static WalletEUR CreateWalletEUR(double bill)
        {
            return new WalletEUR() { Bill = bill };
        }
    }
}
