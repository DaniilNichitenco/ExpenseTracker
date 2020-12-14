using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Dtos.Topics
{
    public class TopicForListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountExpenses { get; set; }
        public bool isGeneral { get; set; }
    }
}
