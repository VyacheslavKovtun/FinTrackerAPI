using FinTrackerAPI.Infrastructure.Business.DTO;
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

        public AuthController(AuthService authService)
        {
            _authService = authService;
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
    }
}
