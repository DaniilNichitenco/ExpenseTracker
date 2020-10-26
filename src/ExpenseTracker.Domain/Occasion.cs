using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Domain
{
    public class Occasion : BaseEntity
    {
        //public ICollection<ProfileOccasion> ProfileOccasions { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
        public DateTime OccasionDate { get; set; } = DateTime.Now;
        public string Title { get; set; }
        public string Context { get; set; }
    }
}
