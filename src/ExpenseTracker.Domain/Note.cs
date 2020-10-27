using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Domain
{
    public class Note : BaseEntity
    {
        public int PersonId { get; set; }
        public Person Person { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
