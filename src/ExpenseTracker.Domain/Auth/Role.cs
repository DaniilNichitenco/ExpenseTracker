using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Domain.Auth
{
    public class Role : IdentityRole<int>
    {
        public Role() : base() { }
        public Role(string roleName) : base(roleName) { }
    }
}
