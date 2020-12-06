using ExpenseTracker.API.Dtos.Expenses;
using ExpenseTracker.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Authorization.IEnumerableExpenseDtoAuthHandler
{
    public class IEnumerableExpenseDtoIsOwnerAuthorizationHandler
        : AuthorizationHandler<OperationAuthorizationRequirement, IEnumerable<ExpenseDto>>
    {

        private readonly IExpenseRepository _repository;
        public IEnumerableExpenseDtoIsOwnerAuthorizationHandler(IExpenseRepository repository)
        {
            _repository = repository;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            OperationAuthorizationRequirement requirement, IEnumerable<ExpenseDto> resourceDto)
        {
            bool permission = true;

            if (context.User == null || resourceDto == null)
            {
                return;
            }

            var resource = _repository.Where(r => resourceDto.Any(dto => dto.Id == r.Id));

            var userId = context.User.Claims.FirstOrDefault(c => c.Type == "UserId");

            foreach (var e in resource)
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
        }
    }
}
