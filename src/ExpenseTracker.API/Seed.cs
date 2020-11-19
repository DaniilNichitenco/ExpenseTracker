using ExpenseTracker.Domain.Auth;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new User()
                {
                    UserName = "lagger179971",
                    Email = "daniilnikitenco1@gmail.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, "123456qwerty123");

                await EnsureRole(userManager, roleManager, user.Id.ToString(), "admin");
            }
        }

        private static async Task<IdentityResult> EnsureRole(UserManager<User> userManager, RoleManager<Role> roleManager, string userId, string role)
        {
            IdentityResult IR = null;

            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new Role(role));
            }

            var user = await userManager.FindByIdAsync(userId);

            if(user == null)
            {
                throw new Exception("The password was probably not strong enough!");
            }

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }
    }
}
