using FinTrackerAPI.Infrastructure.Business.DTO;
using FinTrackerAPI.Services.Interfaces.Models;

namespace FinTrackerAPI.Services.Interfaces.Interfaces
{
    public interface ICurrencyService
    {
        Task<ResponseResult<CurrencyDTO>> CreateAsync(CurrencyDTO currencyDTO);
        Task<ResponseResult<CurrencyDTO>> GetAllAsync();
        Task<ResponseResult<CurrencyDTO>> GetByCodeAsync(string code);
        Task<ResponseResult<CurrencyDTO>> GetByIdAsync(int id);
        Task<ResponseResult<CurrencyDTO>> UpdateAsync(CurrencyDTO currencyDTO);
        Task<ResponseResult<CurrencyDTO>> DeleteAsync(int id);
    }
}
