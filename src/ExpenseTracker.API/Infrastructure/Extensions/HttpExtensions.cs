using ExpenseTracker.Domain.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Infrastructure.Extensions
{
    public static class HttpExtensions
    {
        public static string GetUserIdFromToken(this HttpContext httpContext)
        {
            return httpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
        }
        public static async Task<User> GetUserAsync(this HttpContext httpContext, UserManager<User> userManager)
        {
            var userId = httpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
            return await userManager.FindByIdAsync(userId);
        }
    }
}
