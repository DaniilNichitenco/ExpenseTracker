using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExpenseTracker.API.Dtos.Purses;
using ExpenseTracker.API.Repositories.Interfaces;
using ExpenseTracker.Domain.Purses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PursesController : ControllerBase
    {
        private readonly IPurseRepository _repository;
        private readonly IMapper _mapper;

        public PursesController(IPurseRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPurse(int id)
        {
            var purse = await _repository.Get(id);
            if (purse != null)
            {
                var purseDto = _mapper.Map<PurseDto>(purse);
                return Ok(purseDto);
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPurses()
        {
            var purses = await _repository.GetAll();
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
    }
}
