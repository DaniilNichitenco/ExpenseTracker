using ExpenseTracker.Domain.Auth;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<User> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new User()
                {
                    UserName = "daniil",
                    Email = "daniilnikitenco@gmail.com"
                };

                await userManager.CreateAsync(user, "123456qwerty");
            }
        }
    }
}
