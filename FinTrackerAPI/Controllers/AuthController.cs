using FinTrackerAPI.Infrastructure.Business.DTO;
using FinTrackerAPI.Infrastructure.Data.Repository;
using FinTrackerAPI.Models;
using FinTrackerAPI.Services.Interfaces.Interfaces;
using FinTrackerAPI.Services.Interfaces.Models;
using FinTrackerAPI.Services.Interfaces.Services;
using FinTrackerAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public AuthController(AuthService authService, UserService userService)
        {
            _authService = authService;
            _userService = userService;
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
    }
}
