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

namespace ExpenseTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IPersonRepository _repository;
        private readonly IMapper _mapper;

        public PeopleController(IPersonRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/People
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
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutPerson(int id, Person person)
        //{
        //    if (id != person.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(person).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!PersonExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/People
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Person>> CreatePerson([FromBody]PersonForUpdateDto personForUpdateDto)
        {
            var person = _mapper.Map<Person>(personForUpdateDto);
            await _repository.Add(person);

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

            await _repository.Remove(person);

            return NoContent();
        }

    }
}
