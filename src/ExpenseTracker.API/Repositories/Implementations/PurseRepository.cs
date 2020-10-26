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
    }
}
