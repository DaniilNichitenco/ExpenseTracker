using Microsoft.AspNetCore.Http;
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
    }
}
