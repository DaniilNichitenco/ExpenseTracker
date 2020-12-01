using ExpenseTracker.API.Dtos.Expenses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Authorization.ExpenseDtoAuthHandler
{
    public class ExpenseDtoAdministratorsAuthorizationHandler :
        AuthorizationHandler<OperationAuthorizationRequirement, ExpenseDto>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            OperationAuthorizationRequirement requirement, ExpenseDto resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            var roles = context.User.Claims.Where(c => c.Type == "Role");

            var role = roles.FirstOrDefault(r => r.Value == "admin");

            if (role != null)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
