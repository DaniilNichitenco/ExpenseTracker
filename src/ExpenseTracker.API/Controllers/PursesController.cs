using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExpenseTracker.API.Dtos.Purses;
using ExpenseTracker.API.Repositories.Interfaces;
using ExpenseTracker.Domain.Auth;
using ExpenseTracker.Domain.Purses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PursesController : ControllerBase
    {
        private readonly IPurseRepository _repository;
        private readonly IMapper _mapper;
        private UserManager<User> _userManager;

        public PursesController(IPurseRepository repository, IMapper mapper,
            UserManager<User> userManager)
        {
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPurse(int id)
        {
            var purse = await _repository.Get(id);
            if(purse == null)
            {
                return NotFound();
            }

            var user = await GetUserAsync();

            var purseDto = _mapper.Map<PurseDto>(purse);
            return Ok(purseDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPurses()
        {
            var purses = await _repository.GetAll();
            List<PurseDto> pursesDto = new List<PurseDto>();
            purses.ToList().ForEach(purse => pursesDto.Add(_mapper.Map<PurseDto>(purse)));
            return Ok(pursesDto);
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserPurses(int id)
        {
            var purses = await _repository.Where(p => p.PersonId == id);
            List<PurseDto> pursesDto = new List<PurseDto>();
            purses.ToList().ForEach(purse => pursesDto.Add(_mapper.Map<PurseDto>(purse)));
            return Ok(pursesDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePurse([FromBody] PurseForUpdateDto purseForUpdateDto)
        {
            var purse = PurseFactory.CreateEmptyPurse(purseForUpdateDto.CurrencyCode);

            _mapper.Map(purseForUpdateDto, purse);

            await _repository.Add(purse);
            await _repository.SaveChangesAsync();

            var purseDto = _mapper.Map<PurseDto>(purse);

            return CreatedAtAction(nameof(GetPurse), new { id = purseDto.Id }, purseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePurse(int id, PurseForUpdateDto purseForUpdateDto)
        {
            var purse = await _repository.Get(id);
            if (purse == null)
            {
                return NotFound();
            }

            _mapper.Map(purseForUpdateDto, purse);
            _repository.Update(purse);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurse(int id)
        {
            var purse = await _repository.Get(id);
            if (purse == null)
            {
                return NotFound();
            }

            _repository.Remove(purse);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        private async Task<User> GetUserAsync()
        {
            var id = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
            return await _userManager.FindByIdAsync(id);
        }
    }
}
