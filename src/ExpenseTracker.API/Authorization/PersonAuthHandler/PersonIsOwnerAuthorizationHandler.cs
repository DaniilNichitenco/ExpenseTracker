using ExpenseTracker.Domain;
using ExpenseTracker.Domain.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Authorization.PersonAuthHandler
{
    public class PersonIsOwnerAuthorizationHandler : 
        AuthorizationHandler<OperationAuthorizationRequirement, Person>
    {
        UserManager<User> _userManager;
        public PersonIsOwnerAuthorizationHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        protected override Task HandleRequirementAsync
            (AuthorizationHandlerContext context, 
            OperationAuthorizationRequirement requirement, Person resource)
        {
            if(context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            if(resource.OwnerId.ToString() == _userManager.GetUserId(context.User))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
