using ExpenseTracker.Domain.Purses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpenseTracker.Domain
{
    public class Person : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<Purse> Purses { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
        public virtual ICollection<Occasion> Occasions { get; set; }
    }
}
