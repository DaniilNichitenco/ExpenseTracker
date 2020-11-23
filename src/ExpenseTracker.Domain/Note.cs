using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Domain
{
    public class Note : BaseEntity
    {
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
