using ExpenseTracker.API.Repositories.Interfaces;
using ExpenseTracker.Domain;
using ExpenseTracker.Domain.Auth;
using ExpenseTracker.Domain.Purses;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API
{   
    public class Seed
    {
        public static async Task SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager,
            IPurseRepository purseRepository, IExpenseRepository expenseRepository,
            IUserInfoRepository userInfoRepository, ITopicRepository topicRepository)
        {
            if (!userManager.Users.Any())
            {
                var user = new User()
                {
                    UserName = "lagger179",
                    Email = "daniilnikitenco@gmail.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, "123456qwerty123");

                var IR = await EnsureRole(userManager, roleManager, user.Id.ToString(), "admin");

                if (IR.Succeeded)
                {
                    await SeedPurses(user.Id, purseRepository);
                    await SeedTopics(user.Id, topicRepository);
                    await SeedExpenses(user.Id, expenseRepository);
                    await SeedUserInfo(user.Id, userInfoRepository);
                }
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

        private async static Task SeedPurses(int userId, IPurseRepository repository)
        {
            List<Purse> purses = new List<Purse>()
            {
                new PurseUSD() { OwnerId = userId, Bill = 1000 },
                new PurseEUR() { OwnerId = userId, Bill = 2000 },
            };

            await repository.AddRange(purses);
            await repository.SaveChangesAsync();
        }

        private async static Task SeedTopics(int userId, ITopicRepository repository)
        {
            List<Topic> topics = new List<Topic>()
            {
                new Topic(){ Name = "Food", OwnerId = userId },
                new Topic(){ Name = "Transport", OwnerId = userId },
                new Topic(){ Name = "Amusement", OwnerId = userId },
                new Topic(){ Name = "Others", OwnerId = userId },
        };

            await repository.AddRange(topics);
            await repository.SaveChangesAsync();
        }

        private async static Task SeedUserInfo(int userId, IUserInfoRepository repository)
        {
            UserInfo userInfo = new UserInfo() { OwnerId = userId, FirstName = "Daniil", LastName = "Nichitenco" };

            await repository.Add(userInfo);
            await repository.SaveChangesAsync();
        }

        private async static Task SeedExpenses(int userId, IExpenseRepository repository)
        {
            List<Expense> expenses = new List<Expense>()
            {
                new Expense() { OwnerId = userId, Title="First expense", 
                    Date=new DateTime(2020, 1, 1), Money=300, PurseId=1, TopicId = 1 },
                new Expense() { OwnerId = userId, Title="Second expense", 
                    Date=new DateTime(2020, 1, 2), Money=300, PurseId=1, TopicId = 1 },
                new Expense() { OwnerId = userId, Title="3 expense", 
                    Date=new DateTime(2020, 2, 3), Money=400, PurseId=1, TopicId = 1 },
                new Expense() { OwnerId = userId, Title="4 expense", 
                    Date=new DateTime(2020, 2, 4), Money=400, PurseId=1, TopicId = 2 },
                new Expense() { OwnerId = userId, Title="5 expense", 
                    Date=new DateTime(2020, 3, 5), Money=200, PurseId=1, TopicId = 2 },
                new Expense() { OwnerId = userId, Title="6 expense", 
                    Date=new DateTime(2020, 3, 6), Money=100, PurseId=1, TopicId = 2 },
                new Expense() { OwnerId = userId, Title="7 expense", 
                    Date=new DateTime(2020, 1, 7), Money=400, PurseId=2, TopicId = 3 },
                new Expense() { OwnerId = userId, Title="8 expense", 
                    Date=new DateTime(2020, 1, 8), Money=400, PurseId=2, TopicId = 3 },
                new Expense() { OwnerId = userId, Title="9 expense", 
                    Date=new DateTime(2020, 2, 9), Money=200, PurseId=2, TopicId = 3 },
                new Expense() { OwnerId = userId, Title="10 expense", 
                    Date=new DateTime(2020, 2, 10), Money=100, PurseId=2, TopicId = 4 },
                new Expense() { OwnerId = userId, Title="11 expense", 
                    Date=new DateTime(2020, 3, 11), Money=100, PurseId=2, TopicId = 4 },
                new Expense() { OwnerId = userId, Title="12 expense", 
                    Date=new DateTime(2020, 3, 12), Money=100, PurseId=2, TopicId = 4 },
            };

            await repository.AddRange(expenses);
            await repository.SaveChangesAsync();
        }
    }
}
