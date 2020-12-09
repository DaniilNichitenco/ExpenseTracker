using AutoMapper;
using ExpenseTracker.API.Repositories.Interfaces;
using ExpenseTracker.Domain.Purses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Repositories.Implementations
{
    public class PurseRepository : Repository<Purse>, IPurseRepository
    {
        public PurseRepository(ExpenseTrackerDbContext context, IMapper mapper) : base(context, mapper)
        {

        }

        public List<string> GetAvailablePurses(int userId)
        {
            var purses = Where(p => p.OwnerId == userId);
            //if(purses == null || purses.Count() == 0)
            //{
            //    return new List<string>();
            //}

            var currencies = PurseFactory.GetAllCurrencies();
            foreach(var purse in purses)
            {
                if(currencies.Contains(purse.CurrencyCode.ToLower()))
                {
                    var index = currencies.FindIndex(c => c == purse.CurrencyCode.ToLower());
                    currencies.RemoveAt(index);
                }
            }
            return currencies;
        }

        public int GetAllCurrenciesAmount()
        {
            var amount = PurseFactory.GetAllCurrencies().Count;
            return amount;
        }
    }
}
