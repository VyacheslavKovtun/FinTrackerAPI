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

        [HttpGet("login/token/{authtoken}")]
        public async Task<ResponseResult<UserDTO>> LoginUsingToken(string authtoken)
        {
            var loginViewModelJson = SymmetricEncryption.Decrypt(Convert.FromBase64String(authtoken));
            var loginViewModel = JsonConvert.DeserializeObject<LoginViewModel>(loginViewModelJson);

            return await _authService.LoginAsync(loginViewModel.Email, loginViewModel.Password);
        }

        [HttpPost("register")]
        public async Task<ResponseResult<UserDTO>> Register([FromBody] UserDTO userDTO)
        {
            return await _authService.RegisterAsync(userDTO);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
            ResponseResult<UserDTO> userResponse = await _userService.GetByEmailAsync(email);
            if (userResponse.Code != ResponseResultCode.Success)
                return NotFound(new { message = "User not found or error occurred." });

            UserDTO user = userResponse.Value;

            if (user.IsEmailConfirmed)
                return Ok(new { message = "Email is already confirmed." });

            user.IsEmailConfirmed = true;
            user.UpdatedAt = DateTime.Now;

            await _userService.UpdateAsync(user);
            return Ok(new { message = "Email confirmed successfully!" });
        }
    }
}
