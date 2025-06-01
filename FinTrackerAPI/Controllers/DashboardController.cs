using FinTrackerAPI.Infrastructure.Business.DTO;
using FinTrackerAPI.Services.Interfaces.Interfaces;
using FinTrackerAPI.Services.Interfaces.Models;
using FinTrackerAPI.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace RDTApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private IDashboardService _dashboardService;

        public DashboardController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("financialsummary/{userId}/{days}")]
        [Authorize(Policy = "User")]
        public async Task<ResponseResult<FinancialSummary>> GetFinancialSummary(string userId, int? days)
        {
            return await Task.Run(() => _dashboardService.GetFinancialSummary(userId, days));
        }

        [HttpGet("topexpensecategories/{userId}/{topN}/{days}")]
        [Authorize(Policy = "User")]
        public async Task<ResponseResult<ExpenseCategorySummary>> GetTopExpenseCategories(string userId, int? topN = 10, int? days = 365)
        {
            return await Task.Run(() => _dashboardService.GetTopExpenseCategories(userId, topN, days));
        }

        [HttpGet("monthlybalancetrend/{userId}")]
        [Authorize(Policy = "User")]
        public async Task<ResponseResult<MonthlyBalance>> GetMonthlyBalanceTrend(string userId)
        {
            return await Task.Run(() => _dashboardService.GetMonthlyBalanceTrend(userId));
        }

        [HttpGet("monthlytrend/{userId}/{year}")]
        [Authorize(Policy = "User")]
        public async Task<ResponseResult<MonthlyIncomeExpense>> GetMonthlyIncomeExpenseTrend(string userId, int year)
        {
            return await Task.Run(() => _dashboardService.GetMonthlyIncomeExpenseTrend(userId, year));
        }

        [HttpGet("averagedailyexpense/{userId}/{days}")]
        [Authorize(Policy = "User")]
        public async Task<ResponseResult<DailyExpenseAverage>> GetAverageDailyExpense(string userId, int days)
        {
            return await Task.Run(() => _dashboardService.GetAverageDailyExpense(userId, days));
        }

        [HttpGet("dailyexpenses/{userId}/{days}")]
        [Authorize(Policy = "User")]
        public async Task<ResponseResult<DailyExpense>> GetDailyExpenses(string userId, int? days)
        {
            return await Task.Run(() => _dashboardService.GetDailyExpenses(userId, days));
        }

        [HttpGet("lasttransactions/{userId}/{count}/{days}")]
        [Authorize(Policy = "User")]
        public async Task<ResponseResult<TransactionDTO>> GetLastTransactions(string userId, int? count, int? days)
        {
            return await Task.Run(() => _dashboardService.GetLastTransactions(userId, count, days));
        }
    }
}
