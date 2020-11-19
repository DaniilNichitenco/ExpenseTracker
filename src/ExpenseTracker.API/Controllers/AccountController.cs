using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using ExpenseTracker.API.Dtos.Account;
using ExpenseTracker.API.Dtos.People;
using ExpenseTracker.API.Infrastructure.Configurations;
using ExpenseTracker.API.Repositories.Interfaces;
using ExpenseTracker.Domain;
using ExpenseTracker.Domain.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AuthOptions _authOptions;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        RoleManager<Role> _roleManager;
        IPersonRepository _repository;

        public AccountController(IOptions<AuthOptions> options, SignInManager<User> signInManager,
            UserManager<User> userManager, IMapper mapper, RoleManager<Role> roleManager,
            IPersonRepository repository)
        {
            _authOptions = options.Value;
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _repository = repository;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var checkingPasswordResult = await _signInManager.PasswordSignInAsync(userForLoginDto.Login, userForLoginDto.Password, false, false);
           
            if (checkingPasswordResult.Succeeded)
            {
                var userId = _userManager.FindByNameAsync(userForLoginDto.Login).Id;
                var encodedToken = GetJwtSecurityToken();

                return Ok(new { AccessToken = encodedToken, userId });
            }
            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(UserForSignUpDto userForSignUpDto)
        {
            var user = _mapper.Map<User>(userForSignUpDto);

            await _userManager.CreateAsync(user, userForSignUpDto.Password);

            if(await _userManager.FindByIdAsync(user.Id.ToString()) == null)
            {
                return BadRequest();
            }

            await EnsureRole(user.Id.ToString(), "user");

            var person = _mapper.Map<PersonForUpdateDto>(userForSignUpDto);
            person.OwnerId = user.Id;

            await CreatePerson(person);

            return Ok(new { OwnerId = user.Id});
        }

        private string GetJwtSecurityToken()
        {
            var signingCredentials = new SigningCredentials(_authOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _authOptions.Issuer,
                audience: _authOptions.Audience,
                claims: new List<Claim>(),
                expires: DateTime.Now.AddDays(30),
                signingCredentials: signingCredentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(jwtSecurityToken);
        }

        private async Task<IdentityResult> EnsureRole(string userId, string role)
        {
            IdentityResult IR = null;

            if (!await _roleManager.RoleExistsAsync(role))
            {
                IR = await _roleManager.CreateAsync(new Role(role));
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("The password was probably not strong enough!");
            }

            IR = await _userManager.AddToRoleAsync(user, role);

            return IR;
        }

        private async Task<ActionResult<Person>> CreatePerson(PersonForUpdateDto personForUpdateDto)
        {
            var person = _mapper.Map<Person>(personForUpdateDto);
            await _repository.Add(person);
            await _repository.SaveChangesAsync();

            return Ok();
        }
    }
}
