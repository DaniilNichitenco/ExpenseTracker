using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Domain.Purses
{
    public sealed class PurseFactory
    {
        public static Purse CreateEmptyPurse(string currencyCode)
        {
            
            switch (currencyCode)
            {
                case "MDL":
                    return CreatePurseMDL(0);
                case "USD":
                    return CreatePurseUSD(0);
                case "EUR":
                    return CreatePurseEUR(0);
                default:
                    throw new ArgumentException($"Cannot create {currencyCode} purse");
            }
        }

        private static PurseMDL CreatePurseMDL(double bill)
        {
            return new PurseMDL() { Bill = bill };
        }

        private static PurseUSD CreatePurseUSD(double bill)
        {
            return new PurseUSD() { Bill = bill };
        }

        private static PurseEUR CreatePurseEUR(double bill)
        {
            return new PurseEUR() { Bill = bill };
        }
    }
}
