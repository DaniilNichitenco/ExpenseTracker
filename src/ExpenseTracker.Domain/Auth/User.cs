using ExpenseTracker.Domain.Wallets;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ExpenseTracker.Domain.Auth
{
    public class User : IdentityUser<int>
    {
        public virtual UserInfo UserInfo { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
        public virtual ICollection<Topic> Topics { get; set; }
        public virtual ICollection<Wallet> Wallets { get; set; }
    }
}
