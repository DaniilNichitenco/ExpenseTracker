using ExpenseTracker.API.Infrastructure.Extensions;
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
        private readonly UserManager<User> _userManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserInfoRepository _personRepository;

        public PursesController(IPurseRepository repository, IMapper mapper,
            UserManager<User> userManager, IAuthorizationService authorizationService,
            IUserInfoRepository personRepository)
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
                return Forbid();
            }

            var purseDto = _mapper.Map<PurseDto>(purse);
            return Ok(purseDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPurses()
        {
            var purses = await _repository.GetAll();

            var AR = await _authorizationService.AuthorizeAsync(HttpContext.User, purses, "Permission");
            if (!AR.Succeeded)
            {
                return Forbid();
            }

            List<PurseDto> pursesDto = new List<PurseDto>();
            purses.ToList().ForEach(purse => pursesDto.Add(_mapper.Map<PurseDto>(purse)));
            return Ok(pursesDto);
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserPurses(int userId)
        {
            var purses = _repository.Where(p => p.OwnerId == userId);

            var AR = await _authorizationService.AuthorizeAsync(HttpContext.User, purses, "Permission");
            if (!AR.Succeeded)
            {
                return Forbid();
            }

            List<PurseDto> pursesDto = new List<PurseDto>();
            purses.ToList().ForEach(purse => pursesDto.Add(_mapper.Map<PurseDto>(purse)));
            return Ok(pursesDto);
        }

        [HttpGet("currentUser")]
        public async Task<IActionResult> GetCurrentUserPurses()
        {
            var userId = int.Parse(HttpContext.GetUserIdFromToken());
            var purses = _repository.Where(p => p.OwnerId == userId);

            List<PurseDto> pursesDto = new List<PurseDto>();
            purses.ToList().ForEach(purse => pursesDto.Add(_mapper.Map<PurseDto>(purse)));
            return Ok(pursesDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePurse([FromBody]PurseForCreateDto purseForCreateDto)
        {
            var user = await HttpContext.GetUserAsync(_userManager);
            var purse = PurseFactory.CreateEmptyPurse(purseForCreateDto.CurrencyCode);

            _mapper.Map(purseForCreateDto, purse);
            purse.OwnerId = user.Id;

            await _repository.Add(purse);
            await _repository.SaveChangesAsync();

            var purseDto = _mapper.Map<PurseDto>(purse);

            return CreatedAtAction(nameof(GetPurse), new { id = purseDto.Id }, purseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePurse([FromBody]PurseForUpdateDto purseForUpdateDto, int id)
        {
            var purse = await _repository.Get(id);
            if(purse == null)
            {
                return NotFound();
            }

            var AR = await _authorizationService.AuthorizeAsync(HttpContext.User, purse, "Permission");
            if (!AR.Succeeded)
            {
                return Forbid();
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
                return Forbid();
            }

            _repository.Remove(purse);
            await _repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
