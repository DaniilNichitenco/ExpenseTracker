using AutoMapper;
using ExpenseTracker.API.Repositories.Interfaces;
using ExpenseTracker.Domain.Wallets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Repositories.Implementations
{
    public class WalletRepository : Repository<Wallet>, IWalletRepository
    {
        public WalletRepository(ExpenseTrackerDbContext context, IMapper mapper) : base(context, mapper)
        {

        }

        public List<string> GetAvailableWallets(int userId)
        {
            var Wallets = Where(p => p.OwnerId == userId);

            var currencies = WalletFactory.GetAllCurrencies();
            foreach(var Wallet in Wallets)
            {
                if(currencies.Contains(Wallet.CurrencyCode.ToLower()))
                {
                    var index = currencies.FindIndex(c => c == Wallet.CurrencyCode.ToLower());
                    currencies.RemoveAt(index);
                }
            }
            return currencies;
        }

        public int GetAllCurrenciesAmount()
        {
            var amount = WalletFactory.GetAllCurrencies().Count;
            return amount;
        }
    }
}
