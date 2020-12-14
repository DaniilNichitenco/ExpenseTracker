using ExpenseTracker.API.Repositories.Interfaces;
using ExpenseTracker.Domain;
using ExpenseTracker.Domain.Auth;
using ExpenseTracker.Domain.Wallets;
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
            IWalletRepository WalletRepository, IExpenseRepository expenseRepository,
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
                    await SeedWallets(user.Id, WalletRepository);
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

        private async static Task SeedWallets(int userId, IWalletRepository repository)
        {
            List<Wallet> Wallets = new List<Wallet>()
            {
                new WalletUSD() { OwnerId = userId, Bill = 1000 },
                new WalletEUR() { OwnerId = userId, Bill = 2000 },
            };

            await repository.AddRange(Wallets);
            await repository.SaveChangesAsync();
        }

        private async static Task SeedTopics(int userId, ITopicRepository repository)
        {
            List<Topic> topics = new List<Topic>()
            {
                new Topic(){ Name = "Others", OwnerId = null },
                new Topic(){ Name = "Food", OwnerId = null },
                new Topic(){ Name = "Transport", OwnerId = null },
                new Topic(){ Name = "Health", OwnerId = null },
                new Topic(){ Name = "Sport", OwnerId = userId }
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
                    Date=new DateTime(2020, 1, 1), Money=900, WalletId=1, TopicId = 1 },
                new Expense() { OwnerId = userId, Title="Second expense", 
                    Date=new DateTime(2020, 1, 2), Money=700, WalletId=1, TopicId = 1 },
                new Expense() { OwnerId = userId, Title="3 expense", 
                    Date=new DateTime(2020, 2, 3), Money=1400, WalletId=1, TopicId = 1 },
                new Expense() { OwnerId = userId, Title="4 expense", 
                    Date=new DateTime(2020, 2, 4), Money=800, WalletId=1, TopicId = 2 },
                new Expense() { OwnerId = userId, Title="5 expense", 
                    Date=new DateTime(2020, 3, 5), Money=1200, WalletId=1, TopicId = 2 },
                new Expense() { OwnerId = userId, Title="6 expense", 
                    Date=new DateTime(2020, 3, 6), Money=1100, WalletId=1, TopicId = 2 },
                new Expense() { OwnerId = userId, Title="7 expense", 
                    Date=new DateTime(2020, 1, 7), Money=2400, WalletId=2, TopicId = 3 },
                new Expense() { OwnerId = userId, Title="8 expense", 
                    Date=new DateTime(2020, 1, 8), Money=1400, WalletId=2, TopicId = 3 },
                new Expense() { OwnerId = userId, Title="9 expense", 
                    Date=new DateTime(2020, 2, 9), Money=2200, WalletId=2, TopicId = 3 },
                new Expense() { OwnerId = userId, Title="10 expense", 
                    Date=new DateTime(2020, 2, 10), Money=1200, WalletId=2, TopicId = 4 },
                new Expense() { OwnerId = userId, Title="11 expense", 
                    Date=new DateTime(2020, 3, 11), Money=2100, WalletId=2, TopicId = 4 },
                new Expense() { OwnerId = userId, Title="12 expense", 
                    Date=new DateTime(2020, 3, 12), Money=1100, WalletId=2, TopicId = 4 },
                new Expense() { OwnerId = userId, Title="13 expense",
                    Date=new DateTime(2020, 4, 12), Money=3100, WalletId=2, TopicId = 4 },
                new Expense() { OwnerId = userId, Title="13 expense",
                    Date=new DateTime(2020, 4, 13), Money=1100, WalletId=2, TopicId = 4 },
                new Expense() { OwnerId = userId, Title="14 expense",
                    Date=new DateTime(2020, 4, 14), Money=1400, WalletId=2, TopicId = 4 },
                new Expense() { OwnerId = userId, Title="15 expense",
                    Date=new DateTime(2020, 5, 12), Money=1100, WalletId=2, TopicId = 4 },
                new Expense() { OwnerId = userId, Title="16 expense",
                    Date=new DateTime(2020, 5, 13), Money=1100, WalletId=2, TopicId = 4 },
                new Expense() { OwnerId = userId, Title="17 expense",
                    Date=new DateTime(2020, 5, 14), Money=600, WalletId=2, TopicId = 4 },
                new Expense() { OwnerId = userId, Title="18 expense",
                    Date=new DateTime(2020, 6, 13), Money=600, WalletId=2, TopicId = 4 },
                new Expense() { OwnerId = userId, Title="19 expense",
                    Date=new DateTime(2020, 6, 14), Money=600, WalletId=2, TopicId = 4 },
                new Expense() { OwnerId = userId, Title="20 expense",
                    Date=new DateTime(2020, 6, 15), Money=400, WalletId=2, TopicId = 4 },
                new Expense() { OwnerId = userId, Title="21 expense",
                    Date=new DateTime(2020, 6, 16), Money=270, WalletId=2, TopicId = 4 },
                 new Expense() { OwnerId = userId, Title="22 expense",
                    Date=new DateTime(2020, 12, 8), Money=340, WalletId=1, TopicId = 5 },
                new Expense() { OwnerId = userId, Title="23 expense",
                    Date=new DateTime(2020, 12, 9), Money=90, WalletId=2, TopicId = 1 },
                new Expense() { OwnerId = userId, Title="24 expense",
                    Date=new DateTime(2020, 12, 10), Money=40, WalletId=2, TopicId = 5 },
                new Expense() { OwnerId = userId, Title="10 expense",
                    Date=new DateTime(2020, 12, 10), Money=100, WalletId=2, TopicId = 4 },
                new Expense() { OwnerId = userId, Title="10 expense",
                    Date=new DateTime(2020, 12, 11), Money=90, WalletId=2, TopicId = 4 },
                new Expense() { OwnerId = userId, Title="10 expense",
                    Date=new DateTime(2020, 12, 12), Money=150, WalletId=2, TopicId = 4 },
                new Expense() { OwnerId = userId, Title="10 expense",
                    Date=new DateTime(2020, 12, 13), Money=120, WalletId=2, TopicId = 4 },
                new Expense() { OwnerId = userId, Title="10 expense",
                    Date=new DateTime(2020, 12, 14), Money=200, WalletId=2, TopicId = 4 },
                new Expense() { OwnerId = userId, Title="10 expense",
                    Date=new DateTime(2020, 12, 9), Money=230, WalletId=2, TopicId = 4 },
                new Expense() { OwnerId = userId, Title="10 expense",
                    Date=new DateTime(2020, 12, 8), Money=90, WalletId=2, TopicId = 4 },
                new Expense() { OwnerId = userId, Title="10 expense",
                    Date=new DateTime(2020, 12, 7), Money=40, WalletId=2, TopicId = 4 },
                new Expense() { OwnerId = userId, Title="10 expense",
                    Date=new DateTime(2020, 12, 6), Money=50, WalletId=2, TopicId = 4 },
                new Expense() { OwnerId = userId, Title="10 expense",
                    Date=new DateTime(2020, 12, 5), Money=130, WalletId=2, TopicId = 4 },
                new Expense() { OwnerId = userId, Title="10 expense",
                    Date=new DateTime(2020, 12, 4), Money=170, WalletId=2, TopicId = 4 },
                new Expense() { OwnerId = userId, Title="10 expense",
                    Date=new DateTime(2020, 12, 3), Money=150, WalletId=2, TopicId = 4 },
                new Expense() { OwnerId = userId, Title="10 expense",
                    Date=new DateTime(2020, 12, 2), Money=200, WalletId=2, TopicId = 4 },
                new Expense() { OwnerId = userId, Title="10 expense",
                    Date=new DateTime(2020, 2, 1), Money=100, WalletId=2, TopicId = 4 },
                new Expense() { OwnerId = userId, Title="10 expense",
                    Date=new DateTime(2020, 12, 10), Money=160, WalletId=1, TopicId = 2 },
                new Expense() { OwnerId = userId, Title="10 expense",
                    Date=new DateTime(2020, 12, 11), Money=190, WalletId=1, TopicId = 2 },
                new Expense() { OwnerId = userId, Title="10 expense",
                    Date=new DateTime(2020, 12, 12), Money=120, WalletId=1, TopicId = 2 },
                new Expense() { OwnerId = userId, Title="10 expense",
                    Date=new DateTime(2020, 12, 13), Money=150, WalletId=1, TopicId = 3 },
                new Expense() { OwnerId = userId, Title="10 expense",
                    Date=new DateTime(2020, 12, 14), Money=100, WalletId=1, TopicId = 3 },
                new Expense() { OwnerId = userId, Title="10 expense",
                    Date=new DateTime(2020, 12, 9), Money=130, WalletId=1, TopicId = 3 },
                new Expense() { OwnerId = userId, Title="10 expense",
                    Date=new DateTime(2020, 12, 8), Money=30, WalletId=1, TopicId = 1 },
                new Expense() { OwnerId = userId, Title="10 expense",
                    Date=new DateTime(2020, 12, 7), Money=140, WalletId=1, TopicId = 3 },
                new Expense() { OwnerId = userId, Title="10 expense",
                    Date=new DateTime(2020, 12, 6), Money=150, WalletId=1, TopicId = 5 },
                new Expense() { OwnerId = userId, Title="10 expense",
                    Date=new DateTime(2020, 12, 5), Money=230, WalletId=1, TopicId = 5 },
                new Expense() { OwnerId = userId, Title="10 expense",
                    Date=new DateTime(2020, 12, 4), Money=70, WalletId=1, TopicId = 5 },
                new Expense() { OwnerId = userId, Title="10 expense",
                    Date=new DateTime(2020, 12, 3), Money=50, WalletId=1, TopicId = 1 },
                new Expense() { OwnerId = userId, Title="10 expense",
                    Date=new DateTime(2020, 12, 2), Money=100, WalletId=1, TopicId = 4 },
                new Expense() { OwnerId = userId, Title="10 expense",
                    Date=new DateTime(2020, 2, 1), Money=800, WalletId=1, TopicId = 4 },
                new Expense() { OwnerId = userId, Title="101 expense",
                    Date=new DateTime(2020, 4, 1), Money=1300, WalletId=1, TopicId = 5 },
                new Expense() { OwnerId = userId, Title="102 expense",
                    Date=new DateTime(2020, 5, 1), Money=1800, WalletId=1, TopicId = 5 },
                new Expense() { OwnerId = userId, Title="103 expense",
                    Date=new DateTime(2020, 6, 1), Money=2000, WalletId=1, TopicId = 5 },
                new Expense() { OwnerId = userId, Title="104 expense",
                    Date=new DateTime(2020, 7, 1), Money=2100, WalletId=1, TopicId = 5 },
                new Expense() { OwnerId = userId, Title="105 expense",
                    Date=new DateTime(2020, 8, 1), Money=1800, WalletId=1, TopicId = 3 },
                new Expense() { OwnerId = userId, Title="106 expense",
                    Date=new DateTime(2020, 9, 1), Money=1600, WalletId=1, TopicId = 3 },
                new Expense() { OwnerId = userId, Title="107 expense",
                    Date=new DateTime(2020, 10, 1), Money=1800, WalletId=1, TopicId = 3 },
                new Expense() { OwnerId = userId, Title="108 expense",
                    Date=new DateTime(2020, 11, 1), Money=1900, WalletId=1, TopicId = 3 },
                new Expense() { OwnerId = userId, Title="109 expense",
                    Date=new DateTime(2020, 7, 1), Money=1900, WalletId=2, TopicId = 1 },
                new Expense() { OwnerId = userId, Title="110 expense",
                    Date=new DateTime(2020, 8, 1), Money=800, WalletId=2, TopicId = 2 },
                new Expense() { OwnerId = userId, Title="111 expense",
                    Date=new DateTime(2020, 9, 1), Money=1200, WalletId=2, TopicId = 3 },
                new Expense() { OwnerId = userId, Title="112 expense",
                    Date=new DateTime(2020, 10, 1), Money=1100, WalletId=2, TopicId = 4 },
                new Expense() { OwnerId = userId, Title="113 expense",
                    Date=new DateTime(2020, 11, 1), Money=1500, WalletId=2, TopicId = 5 },
            };

            await repository.AddRange(expenses);
            await repository.SaveChangesAsync();
        }
    }
}
