using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.API.Infrastructure.Extensions;
using ExpenseTracker.API.Repositories.Interfaces;
using AutoMapper;
using ExpenseTracker.API.Dtos.UserDto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using ExpenseTracker.Domain.Auth;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserInfoRepository _repository;
        private readonly IMapper _mapper;
        IAuthorizationService _authorizationService;
        UserManager<User> _userManager;

        public UserController(IUserInfoRepository repository, IMapper mapper, 
            IAuthorizationService authorizationService, UserManager<User> userManager)
        {
            _repository = repository;
            _mapper = mapper;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }

        // GET: api/Users
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var usersInfo = await _repository.GetAll();
            var usersDto = new List<UserDto>();
            usersInfo.ToList().ForEach(async ui => 
            {
                var user = await _userManager.FindByIdAsync(ui.OwnerId.ToString());
                var userDto = _mapper.Map<UserDto>(ui);
                userDto.Email = user.Email;
                userDto.UserName = user.UserName;
                usersDto.Add(userDto);
            });

            return Ok(usersDto);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var userInfo = await _repository.Get(id);

            if (userInfo == null)
            {
                return NotFound();
            }

            var AR = await _authorizationService.AuthorizeAsync(HttpContext.User, userInfo, "Permission");

            if (!AR.Succeeded)
            {
                return Unauthorized();
            }

            UserDto userDto = _mapper.Map<UserDto>(userInfo);
            var user = await HttpContext.GetUserAsync(_userManager);
            userDto.Email = user.Email;
            userDto.UserName = user.UserName;


            return Ok(userDto);
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetUser()
        {
            var user = await HttpContext.GetUserAsync(_userManager);

            var usersInfo = await _repository.Where(p => p.OwnerId == user.Id);
            var userInfo = usersInfo.FirstOrDefault();

            if (userInfo == null)
            {
                return NotFound();
            }

            UserDto userDto = _mapper.Map<UserDto>(userInfo);
            userDto.Email = user.Email;
            userDto.UserName = user.UserName;

            return Ok(userDto);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserInfo(int id, [FromBody]UserForUpdateDto userForUpdateDto)
        {
            var userInfo = await _repository.Get(id);
            if (userInfo == null)
            {
                return NotFound();
            }

            var AR = await _authorizationService.AuthorizeAsync(HttpContext.User, userInfo, "Permission");

            if (!AR.Succeeded)
            {
                return Unauthorized();
            }

            _mapper.Map(userForUpdateDto, userInfo);
            _repository.Update(userInfo);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserInfo([FromBody]UserForUpdateDto userForUpdateDto)
        {
            var userId = int.Parse(HttpContext.GetUserIdFromToken());
            var userInfo = await _repository.Get(userId);
            if (userInfo == null)
            {
                return NotFound();
            }

            _mapper.Map(userForUpdateDto, userInfo);
            _repository.Update(userInfo);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[Authorize(Roles = "admin")]
        //[HttpPost("create/{userId}")]
        //public async Task<ActionResult<UserInfo>> CreatePerson(int userId, [FromBody]UserForUpdateDto personForUpdateDto)
        //{
        //    var user = await GetUserAsync(userId);
        //    if(user == null)
        //    {
        //        return NotFound();
        //    }

        //    var person = _mapper.Map<UserInfo>(personForUpdateDto);
        //    person.OwnerId = userId;

        //    await _repository.Add(person);
        //    await _repository.SaveChangesAsync();

        //    var personDto = _mapper.Map<UserDto>(person);
        //    personDto.Email = user.Email;
        //    personDto.UserName = user.UserName;

        //    return CreatedAtAction(nameof(GetPerson), new { id = personDto.Id }, personDto);
        //}

        // DELETE: api/Users/5
        //[Authorize(Roles = "admin")]
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var person = await _repository.Get(id);

        //    if (person == null)
        //    {
        //        return NotFound();
        //    }

        //    _repository.Remove(person);
        //    await _repository.SaveChangesAsync();

        //    return NoContent();
        //}

        //private async Task<User> GetUserAsync()
        //{
        //    var id = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
        //    return await _userManager.FindByIdAsync(id);
        //}

        //private async Task<User> GetUserAsync(int id)
        //{
        //    return await _userManager.FindByIdAsync(id.ToString());
        //}
    }
}
