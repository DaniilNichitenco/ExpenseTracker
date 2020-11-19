using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.API;
using ExpenseTracker.Domain;
using ExpenseTracker.API.Repositories.Interfaces;
using AutoMapper;
using ExpenseTracker.API.Dtos.People;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using ExpenseTracker.Domain.Auth;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IPersonRepository _repository;
        private readonly IMapper _mapper;
        IAuthorizationService _authorizationService;
        UserManager<User> _userManager;

        public PeopleController(IPersonRepository repository, IMapper mapper, 
            IAuthorizationService authorizationService, UserManager<User> userManager)
        {
            _repository = repository;
            _mapper = mapper;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }

        // GET: api/People
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetPeople()
        {
            var people = await _repository.GetAll();
            var peopleDto = new List<PersonDto>();
            people.ToList().ForEach(p => peopleDto.Add(_mapper.Map<PersonDto>(p)));

            return Ok(peopleDto);
        }

        // GET: api/People/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonById(int id)
        {
            var person = await _repository.Get(id);

            if (person == null)
            {
                return NotFound();
            }

            var AR = await _authorizationService.AuthorizeAsync(HttpContext.User, person, "Permission");

            if (!AR.Succeeded)
            {
                return Unauthorized();
            }

            PersonDto personDto = _mapper.Map<PersonDto>(person);
            var user = await GetUserAsync();
            personDto.Email = user.Email;
            personDto.UserName = user.UserName;


            return Ok(personDto);
        }

        [HttpGet("owner")]
        public async Task<IActionResult> GetPerson()
        {
            var user = await GetUserAsync();

            var people = await _repository.Where(p => p.OwnerId == user.Id);
            var person = people.FirstOrDefault();

            if (person == null)
            {
                return NotFound();
            }

            PersonDto personDto = _mapper.Map<PersonDto>(person);
            personDto.Email = user.Email;
            personDto.UserName = user.UserName;

            return Ok(personDto);
        }

        // PUT: api/People/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(int id, PersonForUpdateDto personForUpdateDto)
        {
            var person = await _repository.Get(id);
            if (person == null)
            {
                return NotFound();
            }

            var AR = await _authorizationService.AuthorizeAsync(HttpContext.User, person, "Permission");

            if (!AR.Succeeded)
            {
                return Unauthorized();
            }

            _mapper.Map(personForUpdateDto, person);
            _repository.Update(person);
            await _repository.SaveChangesAsync();

            return NoContent();
        }



        // POST: api/People
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<Person>> CreatePerson([FromBody] PersonForUpdateDto personForUpdateDto, int userId)
        {
            var user = await GetUserAsync(userId);
            if(user == null)
            {
                return NotFound();
            }

            var person = _mapper.Map<Person>(personForUpdateDto);
            person.OwnerId = userId;

            await _repository.Add(person);
            await _repository.SaveChangesAsync();

            var personDto = _mapper.Map<PersonDto>(person);

            return CreatedAtAction(nameof(GetPerson), new { id = personDto.Id }, personDto);
        }

        // DELETE: api/People/5
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var person = await _repository.Get(id);
            var user = await GetUserAsync(person.OwnerId);

            if (person == null)
            {
                return NotFound();
            }

            if(user != null)
            {
                await _userManager.DeleteAsync(user);
            }


            _repository.Remove(person);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        private async Task<User> GetUserAsync()
        {
            var id = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
            return await _userManager.FindByIdAsync(id);
        }

        private async Task<User> GetUserAsync(int id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }
    }
}
