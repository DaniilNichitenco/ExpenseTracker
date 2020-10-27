using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Dtos.Occasions
{
    public class OccasionDto
    {
        public int Id { get; set; }
        public DateTime OccasionDate { get; set; }
        public string Title { get; set; }
        public string Context { get; set; }
    }
}
