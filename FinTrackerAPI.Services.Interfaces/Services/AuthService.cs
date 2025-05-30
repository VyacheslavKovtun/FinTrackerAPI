using Azure.Core;
using FinTrackerAPI.Domain.Core.Entities;
using FinTrackerAPI.Infrastructure.Business.AutoMapper;
using FinTrackerAPI.Infrastructure.Business.DTO;
using FinTrackerAPI.Infrastructure.Data.Repository;
using FinTrackerAPI.Infrastructure.Data.UnitOfWork;
using FinTrackerAPI.Services.Interfaces.Interfaces;
using FinTrackerAPI.Services.Interfaces.Models;
using FinTrackerAPI.Services.Interfaces.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FinTrackerAPI.Services.Interfaces.Services
{
    public class AuthService : IAuthService
    {
        private IUnitOfWork unitOfWork;
        private AutoMap mapper = AutoMap.Instance;
        //private readonly ILogger<AuthService> _logger;
        private IUserService _userService;
        private IEmailService _emailService;

        public AuthService (IUnitOfWork unitOfWork, UserService userService, EmailService emailService)
        {
            this.unitOfWork = unitOfWork;
            //_logger = logger;
            _userService = userService;
            _emailService = emailService;
        }

        public async Task<bool> Test()
        {
            var token = GenerateUserJWT();
            var handler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = AuthOptions.ISSUER,
                ValidateAudience = true,
                ValidAudience = AuthOptions.USER_AUDIENCE,
                ValidateLifetime = true,
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.FromMinutes(5)
            };

            try
            {
                handler.ValidateToken(token, validationParameters, out var validatedToken);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<ResponseResult<UserDTO>> LoginAsync(string email, string password)
        {
            try
            {
                var userResponse = await _userService.GetByEmailAsync(email);

                if (userResponse.Code == ResponseResultCode.NothingFound)
                    throw new Exception("User was not found!");

                if (userResponse.Code == ResponseResultCode.Failed)
                    throw new Exception("Error loading information from database");

                UserDTO userDTO = userResponse.Value;

                if(!PasswordHelper.VerifyPassword(password, userDTO.PasswordHash))
                    throw new Exception("Wrong password!");

                if (!userDTO.IsEmailConfirmed)
                {
                    if(userDTO.EmailConfirmationToken != null && userDTO.UpdatedAt.HasValue && DateTime.Now < userDTO.UpdatedAt.Value.AddMinutes(5))
                    {
                        throw new Exception("Email is not confirmed. Check your mail to get a confirmation link or try again in 5 minutes to get a new one");
                    }

                    var emailToken = Guid.NewGuid().ToString("N");
                    userDTO.EmailConfirmationToken = emailToken;
                    await _userService.UpdateAsync(userDTO);

                    var confirmationUrl = $"http://localhost:4200/register/confirm-email?userid={Uri.EscapeDataString(userDTO.Id.ToString())}&token={Uri.EscapeDataString(emailToken)}";
                    var sent = await _emailService.SendEmailConfirmationAsync(userDTO.Email, confirmationUrl);

                    throw new Exception("Email is not confirmed. New confirmation link was sent");
                }

                userDTO.APIToken = GenerateUserJWT();
                var updated = await _userService.UpdateAsync(userDTO);

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

        public string GenerateUserJWT()
        {
            var now = DateTime.UtcNow;
            var expires = now.Add(TimeSpan.FromSeconds(AuthOptions.LIFETIME));

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, "user"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, ((DateTimeOffset)now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.USER_AUDIENCE,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: new SigningCredentials(
                    AuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public async Task<ResponseResult<UserDTO>> RegisterAsync(RegisterViewModel registerViewModel)
        {
            try
            {
                var existingUserResponse = await _userService.GetByEmailAsync(registerViewModel.Email);
                if (existingUserResponse.Code == ResponseResultCode.Success)
                    throw new Exception("User with the same email already exists!");

                if (existingUserResponse.Code == ResponseResultCode.Failed)
                    throw new Exception("Error loading information from database");

                var emailToken = Guid.NewGuid().ToString("N");

                var newUserDTO = new UserDTO
                {
                    Id = Guid.NewGuid(),
                    Name = registerViewModel.Name,
                    Email = registerViewModel.Email,
                    PasswordHash = PasswordHelper.HashPassword(registerViewModel.Password),
                    PreferredCurrencyCode = registerViewModel.PreferredCurrencyCode,
                    IsEmailConfirmed = false,
                    EmailConfirmationToken = emailToken
                };

                var user = mapper.Mapper.Map<User>(newUserDTO);

                user.CreatedAt = DateTime.Now;
                await unitOfWork.UserRepository.CreateAsync(user);

                var confirmationUrl = $"http://localhost:4200/register/confirm-email?userid={Uri.EscapeDataString(user.Id.ToString())}&token={Uri.EscapeDataString(emailToken)}";
                var sent = await _emailService.SendEmailConfirmationAsync(user.Email, confirmationUrl);

                return new ResponseResult<UserDTO>
                {
                    Code = ResponseResultCode.Success
                };
            }
            catch (Exception ex)
            {
                //_logger.LogError($"(API_USER_ERROR) CREATE ERROR (User: {userDTO?.LocalUserId}; {userDTO?.CustomerId}): {ex.Message}", ex);
                return new ResponseResult<UserDTO>
                {
                    Code = ResponseResultCode.Failed,
                    Message = ex.Message
                };
            }
        }
    }
}
