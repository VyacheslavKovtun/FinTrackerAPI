using FinTrackerAPI.Infrastructure.Business.DTO;
using FinTrackerAPI.Services.Interfaces.Models;

namespace FinTrackerAPI.Services.Interfaces.Interfaces
{
    public interface ICategoryService
    {
        Task<ResponseResult<CategoryDTO>> CreateAsync(CategoryDTO categoryDTO);
        Task<ResponseResult<CategoryDTO>> GetAllAsync();
        Task<ResponseResult<CategoryDTO>> GetByIdAsync(int id);
        Task<ResponseResult<CategoryDTO>> UpdateAsync(CategoryDTO categoryDTO);
        Task<ResponseResult<CategoryDTO>> DeleteAsync(int id);
    }
}
