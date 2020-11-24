using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExpenseTracker.API.Dtos.Expense;
using ExpenseTracker.API.Infrastructure.Extensions;
using ExpenseTracker.API.Repositories.Interfaces;
using ExpenseTracker.Domain;
using ExpenseTracker.Domain.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseRepository _repository;
        private readonly IMapper _mapper;
        private UserManager<User> _userManager;
        IAuthorizationService _authorizationService;

        public ExpensesController(IExpenseRepository repository, IMapper mapper,
            UserManager<User> userManager, IAuthorizationService authorizationService)
        {
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager;
            _authorizationService = authorizationService;
        }

        [Authorize(Roles = "admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllExpenses()
        {
            var expenses = await _repository.GetAll();
            List<ExpenseDto> expensesDto = new List<ExpenseDto>();
            expenses.ToList().ForEach(expense => expensesDto.Add(_mapper.Map<ExpenseDto>(expense)));
            return Ok(expensesDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExpense(int id)
        {
            var expense = await _repository.Get(id);
            if (expense == null)
            {
                return NotFound();
            }

            var AR = await _authorizationService.AuthorizeAsync(HttpContext.User, expense, "Permission");
            if (!AR.Succeeded)
            {
                return Unauthorized();
            }

            var expenseDto = _mapper.Map<ExpenseDto>(expense);
            return Ok(expenseDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserExpenses()
        {
            var userId = HttpContext.GetUserIdFromToken();

            var expenses = await _repository.Where(e => e.OwnerId.ToString() == userId);

            var AR = await _authorizationService.AuthorizeAsync(HttpContext.User, expenses, "Permission");
            if (!AR.Succeeded)
            {
                return Unauthorized();
            }

            List<ExpenseDto> expensesDto = new List<ExpenseDto>();
            expenses.ToList().ForEach(expense => expensesDto.Add(_mapper.Map<ExpenseDto>(expense)));
            return Ok(expensesDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateExpense([FromBody] ExpenseForCreateDto expenseForCreateDto)
        {
            var userId = HttpContext.GetUserIdFromToken();


            var expense = _mapper.Map<Expense>(expenseForCreateDto);
            expense.OwnerId = int.Parse(userId);

            await _repository.Add(expense);
            await _repository.SaveChangesAsync();

            var expenseDto = _mapper.Map<ExpenseDto>(expense);

            return CreatedAtAction(nameof(GetExpense), new { id = expenseDto.Id }, expenseDto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateExpense(ExpenseForUpdateDto expenseForUpdateDto)
        {
            var expense = await _repository.Get(expenseForUpdateDto.Id);
            if (expense == null)
            {
                return NotFound();
            }

            var AR = await _authorizationService.AuthorizeAsync(HttpContext.User, expense, "Permission");
            if (!AR.Succeeded)
            {
                return Unauthorized();
            }

            _mapper.Map(expenseForUpdateDto, expense);
            _repository.Update(expense);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var expense = await _repository.Get(id);
            if (expense == null)
            {
                return NotFound();
            }

            var AR = await _authorizationService.AuthorizeAsync(HttpContext.User, expense, "Permission");
            if (!AR.Succeeded)
            {
                return Unauthorized();
            }

            _repository.Remove(expense);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("year/{year}")]
        public async Task<IActionResult> GetExpensesForYearlyDiagram(int year)
        {
            var userId = HttpContext.GetUserIdFromToken();

            var expenses = await _repository.GetMonthlyExpenses(int.Parse(userId), year);

            if(expenses == null)
            {
                return NotFound();
            }

            return Ok(new { expenses });
        }
    }
}
