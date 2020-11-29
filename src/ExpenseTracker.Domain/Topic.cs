using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Domain
{
    public class Topic : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
    }
}
