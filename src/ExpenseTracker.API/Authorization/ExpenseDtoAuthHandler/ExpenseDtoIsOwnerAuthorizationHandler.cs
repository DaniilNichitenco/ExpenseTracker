using ExpenseTracker.API.Dtos;
using ExpenseTracker.API.Dtos.Expenses;
using ExpenseTracker.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Authorization.ExpenseDtoAuthHandler
{
    public class ExpenseDtoIsOwnerAuthorizationHandler
        : AuthorizationHandler<OperationAuthorizationRequirement, ExpenseDto>
    {
        private readonly IExpenseRepository _repository;
        public ExpenseDtoIsOwnerAuthorizationHandler(IExpenseRepository repository)
        {
            _repository = repository;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            OperationAuthorizationRequirement requirement, ExpenseDto resourceDto)
        {
            if (context.User == null || resourceDto == null)
            {
                return;
            }

            var resource = await _repository.Get(resourceDto.Id);

            if(resource == null)
            {
                return;
            }

            var userId = context.User.Claims.FirstOrDefault(c => c.Type == "UserId");

            if (resource.OwnerId.ToString() == userId.Value)
            {
                context.Succeed(requirement);
            }
        }
    }
}
