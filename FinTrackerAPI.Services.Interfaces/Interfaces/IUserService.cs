﻿using FinTrackerAPI.Infrastructure.Business.DTO;
using FinTrackerAPI.Services.Interfaces.Models;

namespace FinTrackerAPI.Services.Interfaces.Interfaces
{
    public interface IUserService
    {
        Task<ResponseResult<UserDTO>> CreateAsync(UserDTO userDTO);
        Task<ResponseResult<UserDTO>> GetAllAsync();
        Task<ResponseResult<UserDTO>> GetByIdAsync(string id);
        Task<ResponseResult<UserDTO>> GetByEmailAsync(string email);
        Task<ResponseResult<UserDTO>> UpdateAsync(UserDTO userDTO);
        Task<ResponseResult<UserDTO>> DeleteAsync(string id, string passwordHash);
    }
}
