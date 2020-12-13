using ExpenseTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Repositories.Interfaces
{
    public interface ITopicRepository : IRepository<Topic>
    {
        IEnumerable<Topic> GetTopicsWithFixedExpenses(int count, int userId);
        Task<IEnumerable<string>> GetUserTopicNames(int userId);
        void DeleteTopic(int id);
    }
}
