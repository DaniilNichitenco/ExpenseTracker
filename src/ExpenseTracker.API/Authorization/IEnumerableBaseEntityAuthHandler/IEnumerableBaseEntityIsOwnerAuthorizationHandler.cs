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
    public class IEnumerableBaseEntityIsOwnerAuthorizationHandler :
        AuthorizationHandler<OperationAuthorizationRequirement, IEnumerable<BaseEntity>>
    {
        protected override Task HandleRequirementAsync
            (AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement, IEnumerable<BaseEntity> resource)
        {
            bool permission = true;

            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }


            var userId = context.User.Claims.FirstOrDefault(c => c.Type == "UserId");

            foreach(var e in resource)
            {
                if (e.OwnerId.ToString() != userId.Value)
                {
                    permission = false;
                }
            }

            if (permission)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
