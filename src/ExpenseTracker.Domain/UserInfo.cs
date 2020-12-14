using ExpenseTracker.Domain.Wallets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpenseTracker.Domain
{
    public class UserInfo : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
