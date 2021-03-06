﻿using ExpenseTracker.API.Repositories.Interfaces;
using ExpenseTracker.Domain.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Infrastructure.Extensions
{
    public static class HostExtensions
    {
        public static async Task SeedData(this IHost host)
        {
            using(var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ExpenseTrackerDbContext>();
                    var expenseRepository = services.GetRequiredService<IExpenseRepository>();
                    var WalletRepository = services.GetRequiredService<IWalletRepository>();
                    var userInfoRepository = services.GetRequiredService<IUserInfoRepository>();
                    var topicRepository = services.GetRequiredService<ITopicRepository>();
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var roleManager = services.GetRequiredService<RoleManager<Role>>();

                    context.Database.Migrate();

                    await Seed.SeedUsers(userManager, roleManager, WalletRepository, expenseRepository,
                        userInfoRepository, topicRepository);
                }
                catch(Exception exception)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(exception, "An error occured during migration");
                }
            }
        }
    }
}
