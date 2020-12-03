using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using ExpenseTracker.API.Dtos.Account;
using ExpenseTracker.API.Dtos.UserDto;
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
        private readonly RoleManager<Role> _roleManager;
        private readonly IUserInfoRepository _repository;

        public AccountController(IOptions<AuthOptions> options, SignInManager<User> signInManager,
            UserManager<User> userManager, IMapper mapper, RoleManager<Role> roleManager,
            IUserInfoRepository repository)
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
            Microsoft.AspNetCore.Identity.SignInResult checkingPasswordResult;
            var userByEmail = await _userManager.FindByEmailAsync(userForLoginDto.Login);
            if(userByEmail != null)
            {
                checkingPasswordResult = await _signInManager.PasswordSignInAsync(
                    userByEmail.UserName, userForLoginDto.Password, false, false);
            }
            else
            {
                checkingPasswordResult = await _signInManager.PasswordSignInAsync(
                    userForLoginDto.Login, userForLoginDto.Password, false, false);
            }
           
            if (checkingPasswordResult.Succeeded)
            {
                User user;
                if (userByEmail == null)
                {
                    user = await _userManager.FindByNameAsync(userForLoginDto.Login);
                }
                else
                {
                    user = await _userManager.FindByEmailAsync(userForLoginDto.Login);
                }
                var userId = user.Id;
                var userRoles = await _userManager.GetRolesAsync(user);
                var encodedToken = GetJwtSecurityToken(userId, userRoles as List<string>);

                return Ok(new { AccessToken = encodedToken });
            }
            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(UserForSignUpDto userForSignUpDto)
        {
            var user = _mapper.Map<User>(userForSignUpDto);
            var IR = await _userManager.CreateAsync(user, userForSignUpDto.Password);

            if(!IR.Succeeded)
            {
                return BadRequest(IR.Errors.FirstOrDefault().Description);
            }

            await EnsureRole(user.Id.ToString(), "user");

            var userInfo = _mapper.Map<UserForUpdateDto>(userForSignUpDto);
            await CreateUserInfo(userInfo, user.Id);

            var roles = await _userManager.GetRolesAsync(user);

            var SR = await _signInManager.PasswordSignInAsync(
                user.UserName, userForSignUpDto.Password, false, false);

            if (!SR.Succeeded)
            {
                return BadRequest(new { message = "Cannot sign in this user" });
            }

            var encodedToken = GetJwtSecurityToken(user.Id, roles as List<string>);

            return Ok(new { AccessToken = encodedToken });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return NotFound();
            }

            var people = await _repository.Where(p => p.OwnerId.ToString() == userId);

            _repository.RemoveRange(people);
            await _userManager.DeleteAsync(user);

            return NoContent();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccoutById(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            var people = await _repository.Where(p => p.OwnerId == id);

            _repository.RemoveRange(people);
            await _userManager.DeleteAsync(user);

            return NoContent();
        }

        private string GetJwtSecurityToken(int userId, List<string> roles)
        {
            List<Claim> _claims = new List<Claim>() { new Claim("UserId", userId.ToString()) };
            roles.ForEach(r => _claims.Add(new Claim("Role", r)));
            var signingCredentials = new SigningCredentials(_authOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _authOptions.Issuer,
                audience: _authOptions.Audience,
                claims: _claims,
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
                if (!IR.Succeeded)
                {
                    throw new Exception($"Could not create role {role}");
                }
            }

            var user = await _userManager.FindByIdAsync(userId);

            IR = await _userManager.AddToRoleAsync(user, role);

            return IR;
        }

        private async Task<ActionResult<UserInfo>> CreateUserInfo(UserForUpdateDto userForUpdateDto, int userId)
        {
            var userInfo = _mapper.Map<UserInfo>(userForUpdateDto);
            userInfo.OwnerId = userId;

            await _repository.Add(userInfo);
            await _repository.SaveChangesAsync();

            return Ok(userInfo);
        }
    }
}
