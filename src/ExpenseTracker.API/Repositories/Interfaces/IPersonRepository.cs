using ExpenseTracker.API.Repositories.Inrefaces;
using ExpenseTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Repositories.Interfaces
{
    public interface IPersonRepository : IRepository<Person>
    {
    }
}
