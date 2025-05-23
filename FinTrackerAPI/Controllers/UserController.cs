using FinTrackerAPI.Infrastructure.Business.DTO;
using FinTrackerAPI.Services.Interfaces.Interfaces;
using FinTrackerAPI.Services.Interfaces.Models;
using FinTrackerAPI.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ResponseResult<UserDTO>> GetAllUsers()
        {
            return await _userService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ResponseResult<UserDTO>> GetUserById(string id)
        {
            return await _userService.GetByIdAsync(id);
        }

        [HttpGet("email/{emailaddress}")]
        public async Task<ResponseResult<UserDTO>> GetUserByEmail(string emailaddress)
        {
            return await _userService.GetByEmailAsync(emailaddress);
        }

        [HttpPost("create")]
        public async Task<ResponseResult<UserDTO>> CreateUser([FromBody] UserDTO user)
        {
            return await _userService.CreateAsync(user);
        }

        [HttpPut("update")]
        public async Task<ResponseResult<UserDTO>> UpdateUser([FromBody] UserDTO user)
        {
            return await _userService.UpdateAsync(user);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ResponseResult<UserDTO>> DeleteUser(string id)
        {
            return await _userService.DeleteAsync(id);
        }

    }
}
