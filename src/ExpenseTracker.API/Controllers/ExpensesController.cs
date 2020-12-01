using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExpenseTracker.API.Dtos.Expenses;
using ExpenseTracker.API.Infrastructure.Extensions;
using ExpenseTracker.API.Infrastructure.Models;
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
        private readonly UserManager<User> _userManager;
        private readonly IAuthorizationService _authorizationService;

        public ExpensesController(IExpenseRepository repository, IMapper mapper,
            UserManager<User> userManager, IAuthorizationService authorizationService)
        {
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager;
            _authorizationService = authorizationService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllExpenses()
        {
            var expenses = await _repository.GetAll();

            var AR = await _authorizationService.AuthorizeAsync(HttpContext.User, expenses, "Permission");
            if (!AR.Succeeded)
            {
                return Forbid();
            }

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
                return Forbid();
            }

            var expenseDto = _mapper.Map<ExpenseDto>(expense);
            return Ok(expenseDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserExpenses()
        {
            var userId = HttpContext.GetUserIdFromToken();

            var expenses = await _repository.Where(e => e.OwnerId.ToString() == userId);

            List<ExpenseDto> expensesDto = new List<ExpenseDto>();
            expenses.ToList().ForEach(expense => expensesDto.Add(_mapper.Map<ExpenseDto>(expense)));
            return Ok(expensesDto);
        }

        [HttpPost("PaginatedSearch")]
        public async Task<IActionResult> GetPagedExpenses([FromBody] PagedRequest request)
        {
            var pagedExpensesDto = await _repository.GetPagedData<ExpenseDto>(request);

            var AR = await _authorizationService.AuthorizeAsync(HttpContext.User, pagedExpensesDto.Items.ToList(), "Permission");
            if (!AR.Succeeded)
            {
                return Forbid();
            }

            return Ok(pagedExpensesDto);
        }

        [HttpGet("topic/{topicId}")]
        public async Task<IActionResult> GetUserExpensesByTopicId(int topicId)
        {
            var userId = HttpContext.GetUserIdFromToken();

            var expenses = await _repository.Where(e => e.OwnerId.ToString() == userId && e.TopicId == topicId);

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
                return Forbid();
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
                return Forbid();
            }

            _repository.Remove(expense);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("year")]
        public IActionResult GetExpensesForCurrentYear()
        {
            var userId = HttpContext.GetUserIdFromToken();
            var year = DateTime.Now.Year;

            var expenses = _repository.GetExpensesForYear(int.Parse(userId), year);


            if (expenses == null)
            {
                return NotFound();
            }

            return Ok(new { expenses });
        }

        [HttpGet("sum/year")]
        public async Task<IActionResult> GetSumForCurrentYear()
        {
            var userId = HttpContext.GetUserIdFromToken();
            var year = DateTime.Now.Year;

            var sums = await _repository.GetSumForYear(int.Parse(userId), year);


            if (sums == null)
            {
                return NotFound();
            }

            return Ok(new { sums });
        }

        [HttpGet("sum/month")]
        public async Task<IActionResult> GetSumForCurrentMonth()
        {
            var userId = HttpContext.GetUserIdFromToken();
            var month = DateTime.Now.Year;

            var sums = await _repository.GetSumForMonth(int.Parse(userId), month);


            if (sums == null)
            {
                return NotFound();
            }

            return Ok(new { sums });
        }

        [HttpGet("sum/today")]
        public async Task<IActionResult> GetSumForCurrentDay()
        {
            var userId = HttpContext.GetUserIdFromToken();
            var day = DateTime.Now.Day;

            var sums = await _repository.GetSumForDay(int.Parse(userId), day);


            if (sums == null)
            {
                return NotFound();
            }

            return Ok(new { sums });
        }
    }
}
