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
    public class ListBaseEntityIsOwnerAuthorizationHandler :
        AuthorizationHandler<OperationAuthorizationRequirement, List<BaseEntity>>
    {
        UserManager<User> _userManager;
        public ListBaseEntityIsOwnerAuthorizationHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        protected override Task HandleRequirementAsync
            (AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement, List<BaseEntity> resource)
        {
            bool permission = true;

            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }


            var userId = context.User.Claims.FirstOrDefault(c => c.Type == "UserId");

            resource.ForEach(e =>
            {
                if (e.OwnerId.ToString() != userId.Value)
                {
                    permission = false;
                }
            });

            if (permission)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
