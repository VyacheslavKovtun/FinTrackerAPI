using FinTrackerAPI.Domain.Core.Entities;
using FinTrackerAPI.Infrastructure.Business.AutoMapper;
using FinTrackerAPI.Infrastructure.Business.DTO;
using FinTrackerAPI.Infrastructure.Data.UnitOfWork;
using FinTrackerAPI.Services.Interfaces.Interfaces;
using FinTrackerAPI.Services.Interfaces.Models;

namespace FinTrackerAPI.Services.Interfaces.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork _unitOfWork;
        private AutoMap mapper = AutoMap.Instance;
        //private readonly ILogger<UserService> _logger;

        public UserService(IUnitOfWork unitOfWork/*, ILogger<AppUserService> logger*/)
        {
            _unitOfWork = unitOfWork;
            //_logger = logger;
        }

        public async Task<ResponseResult<UserDTO>> CreateAsync(UserDTO userDTO)
        {
            try
            {
                var user = mapper.Mapper.Map<User>(userDTO);

                user.CreatedAt = DateTime.Now;
                await _unitOfWork.UserRepository.CreateAsync(user);

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

        public async Task<ResponseResult<UserDTO>> DeleteAsync(Guid id)
        {
            try
            {
                await _unitOfWork.UserRepository.DeleteAsync(id);

                return new ResponseResult<UserDTO>
                {
                    Code = ResponseResultCode.Success
                };
            }
            catch (Exception ex)
            {
                //_logger.LogError($"(API_USER_ERROR) DELETE ERROR (User: {id}): {ex.Message}", ex);
                return new ResponseResult<UserDTO>
                {
                    Code = ResponseResultCode.Failed,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseResult<UserDTO>> GetAllAsync()
        {
            try
            {
                var users = await _unitOfWork.UserRepository.GetAllAsync();
                var usersDTO = mapper.Mapper.Map<IEnumerable<UserDTO>>(users);

                if (usersDTO?.Count() <= 0)
                    return new ResponseResult<UserDTO> { Code = ResponseResultCode.NothingFound };

                return new ResponseResult<UserDTO>
                {
                    Code = ResponseResultCode.Success,
                    Values = usersDTO
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

        public async Task<ResponseResult<UserDTO>> GetByIdAsync(Guid id)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetAsync(id);
                var userDTO = mapper.Mapper.Map<UserDTO>(user);

                if (userDTO == null)
                    return new ResponseResult<UserDTO> { Code = ResponseResultCode.NothingFound };

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

        public async Task<ResponseResult<UserDTO>> GetByEmailAsync(string email)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetByEmailAsync(email);
                var UserDTO = mapper.Mapper.Map<UserDTO>(user);

                if (UserDTO == null)
                    return new ResponseResult<UserDTO> { Code = ResponseResultCode.NothingFound };

                return new ResponseResult<UserDTO>
                {
                    Code = ResponseResultCode.Success,
                    Value = UserDTO
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

        public async Task<ResponseResult<UserDTO>> UpdateAsync(UserDTO userDTO)
        {
            try
            {
                var user = mapper.Mapper.Map<User>(userDTO);

                user.UpdatedAt = DateTime.Now;
                await _unitOfWork.UserRepository.UpdateAsync(user);

                return new ResponseResult<UserDTO>
                {
                    Code = ResponseResultCode.Success
                };
            }
            catch (Exception ex)
            {
                //_logger.LogError($"(API_USER_ERROR) UPDATE ERROR (User: {userDTO?.LocalUserId}; {userDTO?.CustomerId}): {ex.Message}", ex);
                return new ResponseResult<UserDTO>
                {
                    Code = ResponseResultCode.Failed,
                    Message = ex.Message
                };
            }
        }
    }
}
