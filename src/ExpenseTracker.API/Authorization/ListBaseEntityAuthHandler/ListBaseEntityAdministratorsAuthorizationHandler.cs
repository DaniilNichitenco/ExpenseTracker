using ExpenseTracker.Domain;
using ExpenseTracker.Domain.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Authorization.ListBaseEntityAuthHandler
{
    public class ListBaseEntityAdministratorsAuthorizationHandler :
        AuthorizationHandler<OperationAuthorizationRequirement, BaseEntity>
    {
        UserManager<User> _userManager;
        public ListBaseEntityAdministratorsAuthorizationHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            OperationAuthorizationRequirement requirement, BaseEntity resource)
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
