using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExpenseTracker.API.Dtos.Occasions;
using ExpenseTracker.API.Repositories.Interfaces;
using ExpenseTracker.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OccasionsController : ControllerBase
    {
        private readonly IOccasionRepository _repository;
        private readonly IMapper _mapper;
        public OccasionsController(IOccasionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOccasion(int id)
        {
            var occasion = await _repository.Get(id);
            if (occasion != null)
            {
                var occasionDto = _mapper.Map<OccasionDto>(occasion);
                return Ok(occasionDto);
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOccasions()
        {
            var occasions = await _repository.GetAll();
            List<OccasionDto> occasionsDto = new List<OccasionDto>();
            occasions.ToList().ForEach(occasion => occasionsDto.Add(_mapper.Map<OccasionDto>(occasions)));
            return Ok(occasionsDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOccasion([FromBody] OccasionForUpdateDto occasionForUpdateDto)
        {
            var occasion = _mapper.Map<Occasion>(occasionForUpdateDto);
            await _repository.Add(occasion);
            await _repository.SaveChangesAsync();

            var occasionDto = _mapper.Map<OccasionDto>(occasion);

            return CreatedAtAction(nameof(GetOccasion), new { id = occasionDto.Id }, occasionDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOccasion(int id, OccasionForUpdateDto occasionForUpdateDto)
        {
            var occasion = await _repository.Get(id);
            if (occasion == null)
            {
                return NotFound();
            }

            _mapper.Map(occasionForUpdateDto, occasion);
            _repository.Update(occasion);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOccasion(int id)
        {
            var occasion = await _repository.Get(id);
            if (occasion == null)
            {
                return NotFound();
            }

            _repository.Remove(occasion);
            await _repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
