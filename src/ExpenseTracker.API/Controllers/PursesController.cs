using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExpenseTracker.API.Dtos.Purses;
using ExpenseTracker.API.Repositories.Interfaces;
using ExpenseTracker.Domain.Auth;
using ExpenseTracker.Domain.Purses;
using Microsoft.AspNetCore.Authorization;
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
        IAuthorizationService _authorizationService;
        IPersonRepository _personRepository;

        public PursesController(IPurseRepository repository, IMapper mapper,
            UserManager<User> userManager, IAuthorizationService authorizationService,
            IPersonRepository personRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager;
            _authorizationService = authorizationService;
            _personRepository = personRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPurse(int id)
        {
            var purse = await _repository.Get(id);
            if(purse == null)
            {
                return NotFound();
            }

            var AR = await _authorizationService.AuthorizeAsync(HttpContext.User, purse, "Permission");
            if (!AR.Succeeded)
            {
                return Unauthorized();
            }

            var purseDto = _mapper.Map<PurseDto>(purse);
            return Ok(purseDto);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllPurses()
        {
            var purses = await _repository.GetAll();
            List<PurseDto> pursesDto = new List<PurseDto>();
            purses.ToList().ForEach(purse => pursesDto.Add(_mapper.Map<PurseDto>(purse)));
            return Ok(pursesDto);
        }

        [HttpGet("person/{id}")]
        public async Task<IActionResult> GetPersonPurses(int id)
        {
            var purses = await _repository.Where(p => p.PersonId == id);

            var AR = await _authorizationService.AuthorizeAsync(HttpContext.User, purses, "Permission");
            if (!AR.Succeeded)
            {
                return Unauthorized();
            }

            List<PurseDto> pursesDto = new List<PurseDto>();
            purses.ToList().ForEach(purse => pursesDto.Add(_mapper.Map<PurseDto>(purse)));
            return Ok(pursesDto);
        }

        [HttpPost("person/{id}")]
        public async Task<IActionResult> CreatePurse([FromBody] PurseForCreateDto purseForCreateDto, int id)
        {
            var person = await _personRepository.Get(id);
            if(person == null)
            {
                return NotFound();
            }

            var AR = await _authorizationService.AuthorizeAsync(HttpContext.User, person, "Permission");
            if (!AR.Succeeded)
            {
                return Unauthorized();
            }

            var user = await GetUserAsync();
            var purse = PurseFactory.CreateEmptyPurse(purseForCreateDto.CurrencyCode);

            _mapper.Map(purseForCreateDto, purse);
            purse.OwnerId = user.Id;

            await _repository.Add(purse);
            await _repository.SaveChangesAsync();

            var purseDto = _mapper.Map<PurseDto>(purse);

            return CreatedAtAction(nameof(GetPurse), new { id = purseDto.Id }, purseDto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePurse(PurseForUpdateDto purseForUpdateDto)
        {
            var purse = await _repository.Get(purseForUpdateDto.Id);
            if(purse == null)
            {
                return NotFound();
            }

            var AR = await _authorizationService.AuthorizeAsync(HttpContext.User, purse, "Permission");
            if (!AR.Succeeded)
            {
                return Unauthorized();
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

            var AR = await _authorizationService.AuthorizeAsync(HttpContext.User, purse, "Permission");
            if (!AR.Succeeded)
            {
                return Unauthorized();
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
