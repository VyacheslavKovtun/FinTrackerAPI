using FinTrackerAPI.Infrastructure.Business.DTO;
using FinTrackerAPI.Services.Interfaces.Models;

namespace FinTrackerAPI.Services.Interfaces.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseResult<UserDTO>> LoginAsync(string email, string password);
        Task<ResponseResult<UserDTO>> RegisterAsync(UserDTO userDTO);
        string GenerateUserJWT();
    }
}
