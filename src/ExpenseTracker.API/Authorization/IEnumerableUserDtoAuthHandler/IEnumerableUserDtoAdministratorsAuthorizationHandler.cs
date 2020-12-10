using ExpenseTracker.API.Dtos.Expenses;
using ExpenseTracker.API.Dtos.UserDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Authorization.IEnumerableUserDtoAuthHandler
{
    public class IEnumerableUserDtoAdministratorsAuthorizationHandler
        : AuthorizationHandler<OperationAuthorizationRequirement, IEnumerable<UserDto>>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            OperationAuthorizationRequirement requirement, IEnumerable<UserDto> resource)
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
