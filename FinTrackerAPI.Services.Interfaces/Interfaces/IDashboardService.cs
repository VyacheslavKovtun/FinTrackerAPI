using FinTrackerAPI.Infrastructure.Business.DTO;
using FinTrackerAPI.Services.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrackerAPI.Services.Interfaces.Interfaces
{
    public interface IDashboardService
    {
        ResponseResult<FinancialSummary> GetFinancialSummary(string userId, int? days);
        ResponseResult<ExpenseCategorySummary> GetTopExpenseCategories(string userId, int topN, int? days);
        ResponseResult<MonthlyBalance> GetMonthlyBalanceTrend(string userId);
        ResponseResult<MonthlyIncomeExpense> GetMonthlyIncomeExpenseTrend(string userId, int? year);
        ResponseResult<DailyExpenseAverage> GetAverageDailyExpense(string userId, int? days);
        ResponseResult<DailyExpense> GetDailyExpenses(string userId, int? days);
        ResponseResult<TransactionDTO> GetLastTransactions(string userId, int? count, int? days);
    }
}
