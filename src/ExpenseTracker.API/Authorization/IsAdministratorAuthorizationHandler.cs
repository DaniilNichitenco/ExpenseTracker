﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Authorization
{
    public class IsAdministratorAuthorizationHandler
        : AuthorizationHandler<OperationAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            OperationAuthorizationRequirement requirement)
        {
            if (context.User == null)
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
