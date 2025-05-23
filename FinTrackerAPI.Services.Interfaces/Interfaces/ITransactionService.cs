using FinTrackerAPI.Infrastructure.Business.DTO;
using FinTrackerAPI.Services.Interfaces.Models;

namespace FinTrackerAPI.Services.Interfaces.Interfaces
{
    public interface ITransactionService
    {
        Task<ResponseResult<TransactionDTO>> CreateAsync(TransactionDTO transactionDTO);
        Task<ResponseResult<TransactionDTO>> GetAllAsync();
        Task<ResponseResult<TransactionDTO>> GetByIdAsync(string id);
        Task<ResponseResult<TransactionDTO>> GetByUserIdAsync(string userId);
        Task<ResponseResult<TransactionDTO>> GetByCategoryAsync(int categoryId);
        Task<ResponseResult<TransactionDTO>> UpdateAsync(TransactionDTO transactionDTO);
        Task<ResponseResult<TransactionDTO>> DeleteAsync(string id);
    }
}
