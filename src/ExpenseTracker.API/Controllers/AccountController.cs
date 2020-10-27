using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ExpenseTracker.API.Dtos.Account;
using ExpenseTracker.API.Infrastructure.Configurations;
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

        public AccountController(IOptions<AuthOptions> options, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _authOptions = options.Value;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var checkingPasswordResult = await _signInManager.PasswordSignInAsync(userForLoginDto.Username, userForLoginDto.Password, false, false);
            var u = new User() { Email = "daniil@gmail.com", UserName = "daniil" };
            await _userManager.CreateAsync(u, "1234qwerty");

            checkingPasswordResult = await _signInManager.PasswordSignInAsync(u.UserName, "1234qwerty", false, false);
            
            if (checkingPasswordResult.Succeeded)
            {
                var encodedToken = GetJwtSecurityToken();

                return Ok(new { AccessToken = encodedToken });
            }
            return Unauthorized();
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
    }
}
