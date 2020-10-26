using AutoMapper;
using ExpenseTracker.API.Repositories.Interfaces;
using ExpenseTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Repositories.Implementations
{
    public class PersonRepository : Repository<Domain.Person>, IPersonRepository
    {
        public PersonRepository(ExpenseTrackerDbContext context, IMapper mapper) : base(context, mapper)
        {

        }
    }
}
