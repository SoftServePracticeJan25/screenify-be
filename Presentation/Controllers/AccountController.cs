using Domain.DTOs.Account;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController(
        UserManager<AppUser> userManager,
        ITokenService tokenService,
        SignInManager<AppUser> signInManager)
        : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var user = await userManager.FindByNameAsync(loginDto.Username);

            if (user == null)
            {
                return Unauthorized(new ValidationProblemDetails(new Dictionary<string, string[]>
                { { "Login", new[] { "Username not found and/or password incorrect." } } }));
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized(new ValidationProblemDetails(new Dictionary<string, string[]>
                { { "Login", new[] {"Username not found and/or password incorrect."} } }));
            }

            if (user.RefreshTokenExpiryDate < DateTime.UtcNow)
            {
                user.RefreshToken = tokenService.CreateRefreshToken();
                user.RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(30);
                await userManager.UpdateAsync(user);
            }

            return Ok(new
            {
                user.UserName,
                user.Email,
                AccessToken = tokenService.CreateAccessToken(user),
                RefreshToken = user.RefreshToken
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var appUser = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                RefreshToken = tokenService.CreateRefreshToken(),
                RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(30),
                Reviews = [],
                Transactions = []
            };

            var createdUser = await userManager.CreateAsync(appUser, registerDto.Password);

            if (!createdUser.Succeeded)
            {
                var errors = createdUser.Errors.ToDictionary(e => e.Code, e => new[] { e.Description });
                return BadRequest(new ValidationProblemDetails(errors));
            }

            var roleResult = await userManager.AddToRoleAsync(appUser, "User");
            if (!roleResult.Succeeded)
            {
                var errors = roleResult.Errors.ToDictionary(e => e.Code, e => new[] { e.Description });
                return BadRequest(new ValidationProblemDetails(errors));
            }

            return Ok(new NewUserDto
            {
                UserName = appUser.UserName,
                Email = appUser.Email,
                AccessToken = tokenService.CreateAccessToken(appUser),
                RefreshToken = appUser.RefreshToken
            });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var user = await userManager.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshTokenDto.RefreshToken && u.RefreshTokenExpiryDate > DateTime.UtcNow);

            if (user == null) return Unauthorized("Invalid or expired refresh token");

            var newAccessToken = tokenService.CreateAccessToken(user);
            var newRefreshToken = tokenService.CreateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(30);

            await userManager.UpdateAsync(user);

            return Ok(new
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

    }
}
