using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Domain
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
