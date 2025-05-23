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
    public class CurrencyController : ControllerBase
    {
        private ICurrencyService _currencyService;

        public CurrencyController(CurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet]
        public async Task<ResponseResult<CurrencyDTO>> GetAllCurrencys()
        {
            return await _currencyService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ResponseResult<CurrencyDTO>> GetCurrencyById(int id)
        {
            return await _currencyService.GetByIdAsync(id);
        }

        [HttpPost("create")]
        public async Task<ResponseResult<CurrencyDTO>> CreateCurrency([FromBody] CurrencyDTO currency)
        {
            return await _currencyService.CreateAsync(currency);
        }

        [HttpPut("update")]
        public async Task<ResponseResult<CurrencyDTO>> UpdateCurrency([FromBody] CurrencyDTO currency)
        {
            return await _currencyService.UpdateAsync(currency);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ResponseResult<CurrencyDTO>> DeleteCurrency(int id)
        {
            return await _currencyService.DeleteAsync(id);
        }

    }
}
