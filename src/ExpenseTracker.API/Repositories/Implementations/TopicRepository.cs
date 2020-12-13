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
            var topics = _context.Set<Topic>()
                .Where(t => t.OwnerId == userId || t.OwnerId == null)
                .Select(t => new Topic()
                { 
                    Id = t.Id,
                    Name = t.Name,
                    Expenses = t.Expenses.Where(e => e.OwnerId == userId)
                        .OrderByDescending(e => e.Date).Take(count).ToList(),
                    OwnerId = t.OwnerId,
                    CreatedAt = t.CreatedAt,
                }).ToList();

            return topics;
        }

        public async Task<IEnumerable<string>> GetUserTopicNames(int userId)
        {
            var topics = Where(t => t.OwnerId == userId);
            var names = topics.Select(t => t.Name);

            return names;
        }

        public void DeleteTopic(int id)
        {
            var topic = _context.Set<Topic>().FirstOrDefault(t => t.Id == id);
            if(topic == null)
            {
                return;
            }
            if(!topic.OwnerId.HasValue)
            {
                return;
            }

            var expenses = topic.Expenses;
            var defaultTopic = _context.Set<Topic>().FirstOrDefault(t => !t.OwnerId.HasValue);
            if(expenses.Count > 0 && defaultTopic != null)
            {
                foreach(var expense in expenses)
                {
                    expense.TopicId = defaultTopic.Id;
                }
            }

            Remove(topic);
        }
    }
}
