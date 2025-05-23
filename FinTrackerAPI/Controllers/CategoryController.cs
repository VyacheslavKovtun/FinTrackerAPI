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
    public class CategoryController : ControllerBase
    {
        private ICategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ResponseResult<CategoryDTO>> GetAllCategorys()
        {
            return await _categoryService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ResponseResult<CategoryDTO>> GetCategoryById(int id)
        {
            return await _categoryService.GetByIdAsync(id);
        }

        [HttpPost("create")]
        public async Task<ResponseResult<CategoryDTO>> CreateCategory([FromBody] CategoryDTO category)
        {
            return await _categoryService.CreateAsync(category);
        }

        [HttpPut("update")]
        public async Task<ResponseResult<CategoryDTO>> UpdateCategory([FromBody] CategoryDTO category)
        {
            return await _categoryService.UpdateAsync(category);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ResponseResult<CategoryDTO>> DeleteCategory(int id)
        {
            return await _categoryService.DeleteAsync(id);
        }

    }
}
