using FinTrackerAPI.Infrastructure.Business.AutoMapper;
using FinTrackerAPI.Infrastructure.Business.DTO;
using FinTrackerAPI.Infrastructure.Data.UnitOfWork;
using FinTrackerAPI.Services.Interfaces.Interfaces;
using FinTrackerAPI.Services.Interfaces.Models;
using FinTrackerAPI.Services.Interfaces.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace FinTrackerAPI.Services.Interfaces.Services
{
    public class AuthService : IAuthService
    {
        private IUnitOfWork unitOfWork;
        private AutoMap mapper = AutoMap.Instance;
        //private readonly ILogger<AuthService> _logger;
        private IUserService _appUserService;

        public AuthService (IUnitOfWork unitOfWork, UserService appUserService)
        {
            this.unitOfWork = unitOfWork;
            //_logger = logger;
            _appUserService = appUserService;
        }

        public async Task<ResponseResult<UserDTO>> LoginAsync(string email, string password)
        {
            try
            {
                UserDTO userDTO = await GetAppUserByEmailLoginData(email, password);

                if (userDTO == null)
                    throw new Exception("User was not found!");

                userDTO.APIToken = GenerateUserJWT();
                var updated = await _appUserService.UpdateAsync(userDTO);

                return new ResponseResult<UserDTO>
                {
                    Code = ResponseResultCode.Success,
                    Value = userDTO
                };
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message, ex);
                return new ResponseResult<UserDTO>
                {
                    Code = ResponseResultCode.Failed,
                    Message = ex.Message
                };
            }
        }

        private async Task<UserDTO> GetAppUserByEmailLoginData(string email, string password)
        {
            var appUser = await unitOfWork.UserRepository.GetByEmailLoginDataAsync(email, password);
            var userDTO = mapper.Mapper.Map<UserDTO>(appUser);

            return userDTO;
        }

        public string GenerateUserJWT()
        {
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.USER_AUDIENCE, null,
                    notBefore: now,
                    expires: now.Add(TimeSpan.FromSeconds(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetUserSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
    }
}
