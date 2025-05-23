using FinTrackerAPI.Domain.Core.Entities;
using FinTrackerAPI.Infrastructure.Business.AutoMapper;
using FinTrackerAPI.Infrastructure.Business.DTO;
using FinTrackerAPI.Infrastructure.Data.UnitOfWork;
using FinTrackerAPI.Services.Interfaces.Interfaces;
using FinTrackerAPI.Services.Interfaces.Models;

namespace FinTrackerAPI.Services.Interfaces.Services
{
    public class CategoryService : ICategoryService
    {
        private IUnitOfWork _unitOfWork;
        private AutoMap mapper = AutoMap.Instance;
        //private readonly ILogger<UserService> _logger;

        public CategoryService(IUnitOfWork unitOfWork/*, ILogger<AppUserService> logger*/)
        {
            _unitOfWork = unitOfWork;
            //_logger = logger;
        }

        public async Task<ResponseResult<CategoryDTO>> CreateAsync(CategoryDTO categoryDTO)
        {
            try
            {
                var category = mapper.Mapper.Map<Category>(categoryDTO);

                await _unitOfWork.CategoryRepository.CreateAsync(category);

                return new ResponseResult<CategoryDTO>
                {
                    Code = ResponseResultCode.Success
                };
            }
            catch (Exception ex)
            {
                //_logger.LogError($"(API_USER_ERROR) CREATE ERROR (Category: {categoryDTO?.LocalUserId}; {categoryDTO?.CustomerId}): {ex.Message}", ex);
                return new ResponseResult<CategoryDTO>
                {
                    Code = ResponseResultCode.Failed,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseResult<CategoryDTO>> DeleteAsync(int id)
        {
            try
            {
                await _unitOfWork.CategoryRepository.DeleteAsync(id);

                return new ResponseResult<CategoryDTO>
                {
                    Code = ResponseResultCode.Success
                };
            }
            catch (Exception ex)
            {
                //_logger.LogError($"(API_USER_ERROR) DELETE ERROR (Category: {id}): {ex.Message}", ex);
                return new ResponseResult<CategoryDTO>
                {
                    Code = ResponseResultCode.Failed,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseResult<CategoryDTO>> GetAllAsync()
        {
            try
            {
                var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
                var usersDTO = mapper.Mapper.Map<IEnumerable<CategoryDTO>>(categories);

                if (usersDTO?.Count() <= 0)
                    return new ResponseResult<CategoryDTO> { Code = ResponseResultCode.NothingFound };

                return new ResponseResult<CategoryDTO>
                {
                    Code = ResponseResultCode.Success,
                    Values = usersDTO
                };
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message, ex);
                return new ResponseResult<CategoryDTO>
                {
                    Code = ResponseResultCode.Failed,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseResult<CategoryDTO>> GetByIdAsync(int id)
        {
            try
            {
                var category = await _unitOfWork.CategoryRepository.GetAsync(id);
                var categoryDTO = mapper.Mapper.Map<CategoryDTO>(category);

                if (categoryDTO == null)
                    return new ResponseResult<CategoryDTO> { Code = ResponseResultCode.NothingFound };

                return new ResponseResult<CategoryDTO>
                {
                    Code = ResponseResultCode.Success,
                    Value = categoryDTO
                };
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message, ex);
                return new ResponseResult<CategoryDTO>
                {
                    Code = ResponseResultCode.Failed,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseResult<CategoryDTO>> UpdateAsync(CategoryDTO categoryDTO)
        {
            try
            {
                var category = mapper.Mapper.Map<Category>(categoryDTO);

                await _unitOfWork.CategoryRepository.UpdateAsync(category);

                return new ResponseResult<CategoryDTO>
                {
                    Code = ResponseResultCode.Success
                };
            }
            catch (Exception ex)
            {
                //_logger.LogError($"(API_USER_ERROR) UPDATE ERROR (Category: {categoryDTO?.LocalUserId}; {categoryDTO?.CustomerId}): {ex.Message}", ex);
                return new ResponseResult<CategoryDTO>
                {
                    Code = ResponseResultCode.Failed,
                    Message = ex.Message
                };
            }
        }
    }
}
