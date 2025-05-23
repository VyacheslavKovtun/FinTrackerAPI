using FinTrackerAPI.Domain.Core.Entities;
using FinTrackerAPI.Infrastructure.Business.AutoMapper;
using FinTrackerAPI.Infrastructure.Business.DTO;
using FinTrackerAPI.Infrastructure.Data.UnitOfWork;
using FinTrackerAPI.Services.Interfaces.Interfaces;
using FinTrackerAPI.Services.Interfaces.Models;

namespace FinTrackerAPI.Services.Interfaces.Services
{
    public class CurrencyService : ICurrencyService
    {
        private IUnitOfWork _unitOfWork;
        private AutoMap mapper = AutoMap.Instance;
        //private readonly ILogger<UserService> _logger;

        public CurrencyService(IUnitOfWork unitOfWork/*, ILogger<AppUserService> logger*/)
        {
            _unitOfWork = unitOfWork;
            //_logger = logger;
        }

        public async Task<ResponseResult<CurrencyDTO>> CreateAsync(CurrencyDTO currencyDTO)
        {
            try
            {
                var currency = mapper.Mapper.Map<Currency>(currencyDTO);

                await _unitOfWork.CurrencyRepository.CreateAsync(currency);

                return new ResponseResult<CurrencyDTO>
                {
                    Code = ResponseResultCode.Success
                };
            }
            catch (Exception ex)
            {
                //_logger.LogError($"(API_USER_ERROR) CREATE ERROR (Currency: {currencyDTO?.LocalUserId}; {currencyDTO?.CustomerId}): {ex.Message}", ex);
                return new ResponseResult<CurrencyDTO>
                {
                    Code = ResponseResultCode.Failed,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseResult<CurrencyDTO>> DeleteAsync(int id)
        {
            try
            {
                await _unitOfWork.CurrencyRepository.DeleteAsync(id);

                return new ResponseResult<CurrencyDTO>
                {
                    Code = ResponseResultCode.Success
                };
            }
            catch (Exception ex)
            {
                //_logger.LogError($"(API_USER_ERROR) DELETE ERROR (Currency: {id}): {ex.Message}", ex);
                return new ResponseResult<CurrencyDTO>
                {
                    Code = ResponseResultCode.Failed,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseResult<CurrencyDTO>> GetAllAsync()
        {
            try
            {
                var currencies = await _unitOfWork.CurrencyRepository.GetAllAsync();
                var currenciesDTO = mapper.Mapper.Map<IEnumerable<CurrencyDTO>>(currencies);

                if (currenciesDTO?.Count() <= 0)
                    return new ResponseResult<CurrencyDTO> { Code = ResponseResultCode.NothingFound };

                return new ResponseResult<CurrencyDTO>
                {
                    Code = ResponseResultCode.Success,
                    Values = currenciesDTO
                };
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message, ex);
                return new ResponseResult<CurrencyDTO>
                {
                    Code = ResponseResultCode.Failed,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseResult<CurrencyDTO>> GetByIdAsync(int id)
        {
            try
            {
                var currency = await _unitOfWork.CurrencyRepository.GetAsync(id);
                var currencyDTO = mapper.Mapper.Map<CurrencyDTO>(currency);

                if (currencyDTO == null)
                    return new ResponseResult<CurrencyDTO> { Code = ResponseResultCode.NothingFound };

                return new ResponseResult<CurrencyDTO>
                {
                    Code = ResponseResultCode.Success,
                    Value = currencyDTO
                };
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message, ex);
                return new ResponseResult<CurrencyDTO>
                {
                    Code = ResponseResultCode.Failed,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseResult<CurrencyDTO>> GetByCodeAsync(string code)
        {
            try
            {
                var currency = await _unitOfWork.CurrencyRepository.GetByCodeAsync(code);
                var currencyDTO = mapper.Mapper.Map<CurrencyDTO>(currency);

                if (currencyDTO == null)
                    return new ResponseResult<CurrencyDTO> { Code = ResponseResultCode.NothingFound };

                return new ResponseResult<CurrencyDTO>
                {
                    Code = ResponseResultCode.Success,
                    Value = currencyDTO
                };
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message, ex);
                return new ResponseResult<CurrencyDTO>
                {
                    Code = ResponseResultCode.Failed,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseResult<CurrencyDTO>> UpdateAsync(CurrencyDTO currencyDTO)
        {
            try
            {
                var currency = mapper.Mapper.Map<Currency>(currencyDTO);

                await _unitOfWork.CurrencyRepository.UpdateAsync(currency);

                return new ResponseResult<CurrencyDTO>
                {
                    Code = ResponseResultCode.Success
                };
            }
            catch (Exception ex)
            {
                //_logger.LogError($"(API_USER_ERROR) UPDATE ERROR (Currency: {currencyDTO?.LocalUserId}; {currencyDTO?.CustomerId}): {ex.Message}", ex);
                return new ResponseResult<CurrencyDTO>
                {
                    Code = ResponseResultCode.Failed,
                    Message = ex.Message
                };
            }
        }
    }
}
