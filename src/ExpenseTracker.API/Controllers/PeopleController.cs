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

namespace ExpenseTracker.API.Controllers
{
    [Route("api/[controller]")]
    //[AllowAnonymous]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IPersonRepository _repository;
        private readonly IMapper _mapper;
        IAuthorizationService _authorizationService;

        public PeopleController(IPersonRepository repository, IMapper mapper, IAuthorizationService authorizationService)
        {
            _repository = repository;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }

        // GET: api/People
        //[Authorize(Roles = "admin")]
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
        public async Task<IActionResult> GetPerson(int id)
        {
            var person = await _repository.Get(id);

            var AR = await _authorizationService.AuthorizeAsync(HttpContext.User, person, "Permission");

            if (!AR.Succeeded)
            {
                return Unauthorized();
            }

            if (person == null)
            {
                return NotFound();
            }

            PersonDto personDto = _mapper.Map<PersonDto>(person);

            return Ok(personDto);
        }

        [HttpGet("owner/{ownerid}")]
        public async Task<IActionResult> GetPersonByOwnerId(int ownerid)
        {
            var people = await _repository.Where(p => p.OwnerId == ownerid);
            var person = people.FirstOrDefault();

            if (person == null)
            {
                return NotFound();
            }

            PersonDto personDto = _mapper.Map<PersonDto>(person);

            return Ok(personDto);
        }

        // PUT: api/People/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(int id, PersonForUpdateDto personForUpdateDto)
        {
            var person = await _repository.Get(id);
            if(person == null)
            {
                return NotFound();
            }

            _mapper.Map(personForUpdateDto, person);
            _repository.Update(person);
            await _repository.SaveChangesAsync();

            return NoContent();
        }



        // POST: api/People
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Person>> CreatePerson([FromBody]PersonForUpdateDto personForUpdateDto)
        {
            var person = _mapper.Map<Person>(personForUpdateDto);
            await _repository.Add(person);
            await _repository.SaveChangesAsync();

            var personDto = _mapper.Map<PersonDto>(person);
            
            return CreatedAtAction(nameof(GetPerson), new { id = personDto.Id }, personDto);
        }

        // DELETE: api/People/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var person = await _repository.Get(id);
            if (person == null)
            {
                return NotFound();
            }

            _repository.Remove(person);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

    }
}
