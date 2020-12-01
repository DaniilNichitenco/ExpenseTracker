using ExpenseTracker.Domain;
using ExpenseTracker.Domain.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Authorization.IEnumerableBaseEntityAuthHandler
{
    public class IEnumerableBaseEntityAdministratorsAuthorizationHandler :
        AuthorizationHandler<OperationAuthorizationRequirement, IEnumerable<BaseEntity>>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            OperationAuthorizationRequirement requirement, IEnumerable<BaseEntity> resource)
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
