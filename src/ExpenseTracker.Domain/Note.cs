using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Domain
{
    public class Note : BaseEntity
    {
        //public virtual ICollection<ProfileNote> ProfileNotes { get; set; }
        public int ProfileId { get; set; }
        public Profile Profile { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
