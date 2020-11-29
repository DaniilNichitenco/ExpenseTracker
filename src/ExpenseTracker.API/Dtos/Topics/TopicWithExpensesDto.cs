using ExpenseTracker.API.Dtos.Expenses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Dtos.Topics
{
    public class TopicWithExpensesDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ExpenseDto> Expenses { get; set; }
    }
}
