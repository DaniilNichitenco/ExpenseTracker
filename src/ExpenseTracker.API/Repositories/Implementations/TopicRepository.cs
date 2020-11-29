using AutoMapper;
using ExpenseTracker.API.Repositories.Interfaces;
using ExpenseTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Repositories.Implementations
{
    public class TopicRepository : Repository<Topic>, ITopicRepository
    {
        public TopicRepository(ExpenseTrackerDbContext context, IMapper mapper) : base(context, mapper)
        {

        }

        public IEnumerable<Topic> GetTopicsWithFixedExpenses(int count, int userId)
        {
            var topics = _context.Set<Topic>().Select(t => new Topic()
            { 
                Id = t.Id,
                Name = t.Name,
                Expenses = t.Expenses.Where(e => e.OwnerId == userId).Take(count).ToList(),
                OwnerId = t.OwnerId,
                CreatedAt = t.CreatedAt,
            }).ToList();

            return topics;
        }
    }
}
