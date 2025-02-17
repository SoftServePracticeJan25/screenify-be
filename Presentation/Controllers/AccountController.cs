using Domain.DTOs.Account;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Extentions;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController(
        UserManager<AppUser> userManager,
        ITokenService tokenService,
        IUserInfoService userInfoService,
        SignInManager<AppUser> signInManager,
        ISendGridEmailService emailService,
        IConfiguration configuration)
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

            var roles = await userManager.GetRolesAsync(user); 

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
                AccessToken = tokenService.CreateAccessToken(user, roles.ToList()), 
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

            var token = await userManager.GenerateUserTokenAsync(appUser, TokenOptions.DefaultProvider, "EmailConfirmation");
            Console.WriteLine(token);
            var confirmationLink = $"{configuration["BaseUrl"]}/api/account/confirm-email?userId={appUser.Id}&token={Uri.EscapeDataString(token)}";

            // Sending confirmation email
            await emailService.SendEmailConfirmationAsync(appUser.Email, confirmationLink);

            var roles = await userManager.GetRolesAsync(appUser); 

            return Ok(new NewUserDto
            {
                UserName = appUser.UserName,
                Email = appUser.Email,
                AccessToken = tokenService.CreateAccessToken(appUser, roles.ToList()), 
                RefreshToken = appUser.RefreshToken
            });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            if (string.IsNullOrWhiteSpace(refreshTokenDto.RefreshToken))
            {
                return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
                { { "RefreshToken", new[] {"Refresh token is required."} } }));
            }

            var user = await userManager.Users
                .SingleOrDefaultAsync(u => u.RefreshToken == refreshTokenDto.RefreshToken && u.RefreshTokenExpiryDate > DateTime.UtcNow);

            if (user == null)
            {
                return Unauthorized(new ValidationProblemDetails(new Dictionary<string, string[]>
                { { "RefreshToken", new[] {"Invalid or expired refresh token."} } }));
            }


            var roles = await userManager.GetRolesAsync(user);

            
            var newAccessToken = tokenService.CreateAccessToken(user, roles.ToList());

            user.RefreshToken = tokenService.CreateRefreshToken();
            user.RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(30);

            await userManager.UpdateAsync(user);

            return Ok(new
            {
                AccessToken = newAccessToken,
                RefreshToken = user.RefreshToken
            });
        }

        [HttpGet("user-info")]
        [Authorize]
        public async Task<IActionResult> GetUserInfo()
        {
            var username = User.GetUsername();
            var appUser = await userManager.FindByNameAsync(username);
            if (appUser == null)
            {
                return NotFound();
            }

            UserInfoDto userInfo = await userInfoService.GetUserInfo(appUser);

            return Ok(userInfo);
        }

        [HttpGet("get-users")]
        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> GetUsers()
        {
            var users = await userInfoService.GetAllUsersAsync();
            return new JsonResult(users);
        }


        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return BadRequest("Invalid user.");

            var result = await userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded) return BadRequest("Email confirmation failed.");

            return Ok("Email confirmed successfully!");
        }
        
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { error = "Invalid token. No user ID found." });

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new { error = "User not found." });

            // Validating new password
            var passwordValidator = new PasswordValidator<AppUser>();
            var passwordResult = await passwordValidator.ValidateAsync(userManager, user, changePasswordDto.NewPassword);

            if (!passwordResult.Succeeded)
            {
                var errors = passwordResult.Errors.Select(e => e.Description).ToList();
                return BadRequest(new { errors });
            }

            var changePasswordResult = await userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                var errors = changePasswordResult.Errors.Select(e => e.Description).ToList();
                return BadRequest(new { errors });
            }

            return Ok(new { message = "Password changed successfully." });
        }
        
        [HttpPost("change-username")]
        [Authorize]
        public async Task<IActionResult> ChangeUsername([FromBody] ChangeUsernameDto changeUsernameDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("Invalid token. No user ID found.");

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return Unauthorized(new { message = "User not found." });

            // New login must be free
            var existingUser = await userManager.FindByNameAsync(changeUsernameDto.NewUsername);
            if (existingUser != null)
            {
                return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
                { { "NewUsername", new[] { "This username is already taken." } } }));
            }

            // New login must not be the same as old one
            if (user.UserName == changeUsernameDto.NewUsername)
            {
                return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
                { { "NewUsername", new[] { "New username must be different from the current username." } } }));
            }

            var setUsernameResult = await userManager.SetUserNameAsync(user, changeUsernameDto.NewUsername);
            if (!setUsernameResult.Succeeded)
            {
                var errors = setUsernameResult.Errors.ToDictionary(e => e.Code, e => new[] { e.Description });
                return BadRequest(new ValidationProblemDetails(errors));
            }

            // Generating new tokens
            var roles = await userManager.GetRolesAsync(user);
            user.RefreshToken = tokenService.CreateRefreshToken();
            user.RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(30);
            await userManager.UpdateAsync(user);

            return Ok(new
            {
                NewUsername = user.UserName,
                AccessToken = tokenService.CreateAccessToken(user, roles.ToList()),
                RefreshToken = user.RefreshToken
            });
        }

    }
}
