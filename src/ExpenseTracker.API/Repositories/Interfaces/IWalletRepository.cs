using ExpenseTracker.Domain.Wallets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Repositories.Interfaces
{
    public interface IWalletRepository : IRepository<Wallet>
    {
        List<string> GetAvailableWallets(int userId);
        int GetAllCurrenciesAmount();
    }
}
