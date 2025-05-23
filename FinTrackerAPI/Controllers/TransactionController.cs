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
    public class TransactionController : ControllerBase
    {
        private ITransactionService _transactionService;

        public TransactionController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<ResponseResult<TransactionDTO>> GetAllTransactions()
        {
            return await _transactionService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ResponseResult<TransactionDTO>> GetTransactionById(string id)
        {
            return await _transactionService.GetByIdAsync(id);
        }

        [HttpGet("user/{userId}")]
        public async Task<ResponseResult<TransactionDTO>> GetTransactionByUserId(string userId)
        {
            return await _transactionService.GetByUserIdAsync(userId);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ResponseResult<TransactionDTO>> GetTransactionByCategoryId(int categoryId)
        {
            return await _transactionService.GetByCategoryAsync(categoryId);
        }

        [HttpPost("create")]
        public async Task<ResponseResult<TransactionDTO>> CreateTransaction([FromBody] TransactionDTO transaction)
        {
            return await _transactionService.CreateAsync(transaction);
        }

        [HttpPut("update")]
        public async Task<ResponseResult<TransactionDTO>> UpdateTransaction([FromBody] TransactionDTO transaction)
        {
            return await _transactionService.UpdateAsync(transaction);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ResponseResult<TransactionDTO>> DeleteTransaction(string id)
        {
            return await _transactionService.DeleteAsync(id);
        }

    }
}
