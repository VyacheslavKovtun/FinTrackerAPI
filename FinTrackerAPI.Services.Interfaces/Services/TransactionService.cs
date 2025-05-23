using FinTrackerAPI.Domain.Core.Entities;
using FinTrackerAPI.Infrastructure.Business.AutoMapper;
using FinTrackerAPI.Infrastructure.Business.DTO;
using FinTrackerAPI.Infrastructure.Data.UnitOfWork;
using FinTrackerAPI.Services.Interfaces.Interfaces;
using FinTrackerAPI.Services.Interfaces.Models;

namespace FinTrackerAPI.Services.Interfaces.Services
{
    public class TransactionService : ITransactionService
    {
        private IUnitOfWork _unitOfWork;
        private AutoMap mapper = AutoMap.Instance;
        //private readonly ILogger<UserService> _logger;

        public TransactionService(IUnitOfWork unitOfWork/*, ILogger<AppUserService> logger*/)
        {
            _unitOfWork = unitOfWork;
            //_logger = logger;
        }

        public async Task<ResponseResult<TransactionDTO>> CreateAsync(TransactionDTO transactionDTO)
        {
            try
            {
                var transaction = mapper.Mapper.Map<Transaction>(transactionDTO);

                transaction.Date = DateTime.Now;
                await _unitOfWork.TransactionRepository.CreateAsync(transaction);

                return new ResponseResult<TransactionDTO>
                {
                    Code = ResponseResultCode.Success
                };
            }
            catch (Exception ex)
            {
                //_logger.LogError($"(API_USER_ERROR) CREATE ERROR (Transaction: {transactionDTO?.LocalUserId}; {transactionDTO?.CustomerId}): {ex.Message}", ex);
                return new ResponseResult<TransactionDTO>
                {
                    Code = ResponseResultCode.Failed,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseResult<TransactionDTO>> DeleteAsync(string id)
        {
            try
            {
                var gId = new Guid(id);
                await _unitOfWork.TransactionRepository.DeleteAsync(gId);

                return new ResponseResult<TransactionDTO>
                {
                    Code = ResponseResultCode.Success
                };
            }
            catch (Exception ex)
            {
                //_logger.LogError($"(API_USER_ERROR) DELETE ERROR (Transaction: {id}): {ex.Message}", ex);
                return new ResponseResult<TransactionDTO>
                {
                    Code = ResponseResultCode.Failed,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseResult<TransactionDTO>> GetAllAsync()
        {
            try
            {
                var transactions = await _unitOfWork.TransactionRepository.GetAllAsync();
                var transactionsDTO = mapper.Mapper.Map<IEnumerable<TransactionDTO>>(transactions);

                if (transactionsDTO?.Count() <= 0)
                    return new ResponseResult<TransactionDTO> { Code = ResponseResultCode.NothingFound };

                return new ResponseResult<TransactionDTO>
                {
                    Code = ResponseResultCode.Success,
                    Values = transactionsDTO
                };
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message, ex);
                return new ResponseResult<TransactionDTO>
                {
                    Code = ResponseResultCode.Failed,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseResult<TransactionDTO>> GetByCategoryAsync(int categoryId)
        {
            try
            {
                var transactions = await _unitOfWork.TransactionRepository.GetByCategoryAsync(categoryId);
                var transactionsDTO = mapper.Mapper.Map<IEnumerable<TransactionDTO>>(transactions);

                if (transactionsDTO?.Count() <= 0)
                    return new ResponseResult<TransactionDTO> { Code = ResponseResultCode.NothingFound };

                return new ResponseResult<TransactionDTO>
                {
                    Code = ResponseResultCode.Success,
                    Values = transactionsDTO
                };
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message, ex);
                return new ResponseResult<TransactionDTO>
                {
                    Code = ResponseResultCode.Failed,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseResult<TransactionDTO>> GetByIdAsync(string id)
        {
            try
            {
                var gId = new Guid(id);
                var transaction = await _unitOfWork.TransactionRepository.GetAsync(gId);
                var transactionDTO = mapper.Mapper.Map<TransactionDTO>(transaction);

                if (transactionDTO == null)
                    return new ResponseResult<TransactionDTO> { Code = ResponseResultCode.NothingFound };

                return new ResponseResult<TransactionDTO>
                {
                    Code = ResponseResultCode.Success,
                    Value = transactionDTO
                };
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message, ex);
                return new ResponseResult<TransactionDTO>
                {
                    Code = ResponseResultCode.Failed,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseResult<TransactionDTO>> GetByUserIdAsync(string userId)
        {
            try
            {
                var gId = new Guid(userId);
                var transactions = await _unitOfWork.TransactionRepository.GetByUserIdAsync(gId);
                var transactionsDTO = mapper.Mapper.Map<IEnumerable<TransactionDTO>>(transactions);

                if (transactionsDTO?.Count() <= 0)
                    return new ResponseResult<TransactionDTO> { Code = ResponseResultCode.NothingFound };

                return new ResponseResult<TransactionDTO>
                {
                    Code = ResponseResultCode.Success,
                    Values = transactionsDTO
                };
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message, ex);
                return new ResponseResult<TransactionDTO>
                {
                    Code = ResponseResultCode.Failed,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseResult<TransactionDTO>> UpdateAsync(TransactionDTO transactionDTO)
        {
            try
            {
                var transaction = mapper.Mapper.Map<Transaction>(transactionDTO);

                await _unitOfWork.TransactionRepository.UpdateAsync(transaction);

                return new ResponseResult<TransactionDTO>
                {
                    Code = ResponseResultCode.Success
                };
            }
            catch (Exception ex)
            {
                //_logger.LogError($"(API_USER_ERROR) UPDATE ERROR (Transaction: {transactionDTO?.LocalUserId}; {transactionDTO?.CustomerId}): {ex.Message}", ex);
                return new ResponseResult<TransactionDTO>
                {
                    Code = ResponseResultCode.Failed,
                    Message = ex.Message
                };
            }
        }
    }
}
