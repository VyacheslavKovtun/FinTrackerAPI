using FinTrackerAPI.Infrastructure.Business.DTO;
using FinTrackerAPI.Infrastructure.Data.Repository;
using FinTrackerAPI.Models;
using FinTrackerAPI.Services.Interfaces.Interfaces;
using FinTrackerAPI.Services.Interfaces.Models;
using FinTrackerAPI.Services.Interfaces.Services;
using FinTrackerAPI.Services.Interfaces.Utils;
using FinTrackerAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace FinTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        private IUserService _userService;
        private IEmailService _emailService;
        private IMemoryCache _cache;

        public AuthController(AuthService authService, UserService userService, EmailService emailService, IMemoryCache cache)
        {
            _authService = authService;
            _userService = userService;
            _emailService = emailService;
            _cache = cache;
        }

        [HttpPost("login")]
        public async Task<ResponseResult<UserDTO>> Login([FromBody] LoginViewModel loginViewModel)
        {
            return await _authService.LoginAsync(loginViewModel.Email, loginViewModel.Password);
        }

        [HttpPost("register")]
        public async Task<ResponseResult<UserDTO>> Register([FromBody] RegisterViewModel registerViewModel)
        {
            return await _authService.RegisterAsync(registerViewModel);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userid, [FromQuery] string token)
        {
            ResponseResult<UserDTO> userResponse = await _userService.GetByIdAsync(userid);
            if (userResponse.Code != ResponseResultCode.Success)
                return NotFound(new { message = "User not found or error occurred." });

            UserDTO user = userResponse.Value;

            if (user.IsEmailConfirmed)
                return Ok(new { message = "Email is already confirmed." });

            if (user.EmailConfirmationToken != token)
                return BadRequest(new { message = "Invalid confirmation token." });

            user.IsEmailConfirmed = true;
            user.EmailConfirmationToken = null;
            user.UpdatedAt = DateTime.Now;

            await _userService.UpdateAsync(user);
            return Ok(new { message = "Email confirmed successfully!" });
        }

        [HttpGet("send-reset-password")]
        public async Task<IActionResult> SendResetPasswordLink([FromQuery] string email)
        {
            var userResponse = await _userService.GetByEmailAsync(email);
            if (userResponse.Code != ResponseResultCode.Success)
                return NotFound(new { message = "User not found." });

            var user = userResponse.Value;

            var token = Guid.NewGuid().ToString();

            _cache.Set(token, user.Id, TimeSpan.FromMinutes(10));

            var resetUrl = $"http://localhost:4200/reset-password?token={token}&userid={user.Id}";

            var sent = await _emailService.SendEmailConfirmationAsync(email, resetUrl);

            return Ok(new { message = "Reset link sent." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel request)
        {
            if (!_cache.TryGetValue(request.Token, out Guid userId))
            {
                return BadRequest(new { message = "Invalid or expired token." });
            }

            var userResult = await _userService.GetByIdAsync(userId.ToString());
            if (userResult.Code != ResponseResultCode.Success)
            {
                return NotFound(new { message = "User not found." });
            }

            var user = userResult.Value;
            user.PasswordHash = PasswordHelper.HashPassword(request.NewPassword);
            user.UpdatedAt = DateTime.Now;

            await _userService.UpdateAsync(user);

            _cache.Remove(request.Token);

            return Ok(new { message = "Password has been reset successfully." });
        }
    }
}
