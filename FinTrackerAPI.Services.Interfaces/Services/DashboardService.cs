using FinTrackerAPI.Infrastructure.Business.DTO;
using FinTrackerAPI.Services.Interfaces.Interfaces;
using FinTrackerAPI.Services.Interfaces.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FinTrackerAPI.Services.Interfaces.Services
{
    public class DashboardService : IDashboardService
    {
        public DashboardService()
        {
        }

        private string GetConnectionString()
        {
            return "Data Source=DESKTOP-A49B8TD\\MSSQLSERVER01;Initial Catalog=FinTracker;User ID=finUser;Password=Password8;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";
        }

        public ResponseResult<FinancialSummary> GetFinancialSummary(string userId, int? days = 365)
        {
            try
            {
                var financial = new FinancialSummary();

                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("GetFinancialSummary", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                    cmd.Parameters.AddWithValue("@Days", days);
                    cmd.CommandTimeout = 90;

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            financial = new FinancialSummary() {
                               TotalIncome = rdr["TotalIncome"]?.ToString() != String.Empty ? decimal.Parse(rdr["TotalIncome"]?.ToString()) : decimal.Zero,
                               TotalExpense = rdr["TotalExpense"]?.ToString() != String.Empty ? decimal.Parse(rdr["TotalExpense"]?.ToString()) : decimal.Zero,
                               Savings = rdr["Savings"]?.ToString() != String.Empty ? decimal.Parse(rdr["Savings"]?.ToString()) : decimal.Zero,
                               SavingsRate = rdr["SavingsRate"]?.ToString() != String.Empty ? decimal.Parse(rdr["SavingsRate"]?.ToString()) : decimal.Zero
                            };
                        }
                    }

                    conn.Close();
                }

                return new ResponseResult<FinancialSummary>
                {
                    Code = ResponseResultCode.Success,
                    Value = financial
                };
            }
            catch (Exception ex)
            {
                /*_logger.LogError(ex.Message, ex);*/
                return new ResponseResult<FinancialSummary>
                {
                    Code = ResponseResultCode.Failed,
                    Message = ex.Message
                };
            }
        }

        public ResponseResult<ExpenseCategorySummary> GetTopExpenseCategories(string userId, int topN = 10, int? days = 30)
        {
            var result = new List<ExpenseCategorySummary>();
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();
                    var cmd = new SqlCommand("GetTopExpenseCategories", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                    cmd.Parameters.Add(new SqlParameter("@TopN", topN));
                    cmd.Parameters.AddWithValue("@Days", days);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            result.Add(new ExpenseCategorySummary
                            {
                                Category = rdr["Category"].ToString(),
                                Total = decimal.Parse(rdr["Total"].ToString())
                            });
                        }
                    }
                }

                return new ResponseResult<ExpenseCategorySummary> { Code = ResponseResultCode.Success, Values = result };
            }
            catch (Exception ex)
            {
                return new ResponseResult<ExpenseCategorySummary> { Code = ResponseResultCode.Failed, Message = ex.Message };
            }
        }

        public ResponseResult<MonthlyBalance> GetMonthlyBalanceTrend(string userId)
        {
            var result = new List<MonthlyBalance>();
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();
                    var cmd = new SqlCommand("GetMonthlyBalanceTrend", conn) { CommandType = CommandType.StoredProcedure };
                    cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            result.Add(new MonthlyBalance
                            {
                                Month = rdr["Month"].ToString(),
                                NetBalance = decimal.Parse(rdr["NetBalance"].ToString())
                            });
                        }
                    }
                }

                return new ResponseResult<MonthlyBalance> { Code = ResponseResultCode.Success, Values = result };
            }
            catch (Exception ex)
            {
                return new ResponseResult<MonthlyBalance> { Code = ResponseResultCode.Failed, Message = ex.Message };
            }
        }

        public ResponseResult<MonthlyIncomeExpense> GetMonthlyIncomeExpenseTrend(string userId, int? year)
        {
            try
            {
                var result = new List<MonthlyIncomeExpense>();

                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    var cmd = new SqlCommand("GetMonthlyIncomeExpenseTrend", conn)
                    {
                        CommandType = CommandType.StoredProcedure,
                        CommandTimeout = 90
                    };

                    if (year == null)
                        year = DateTime.Now.Year;

                    cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                    cmd.Parameters.AddWithValue("@Year", year);

                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            result.Add(new MonthlyIncomeExpense
                            {
                                Month = rdr["Month"]?.ToString(),
                                Income = rdr["Income"] != DBNull.Value ? Convert.ToDecimal(rdr["Income"]) : 0,
                                Expense = rdr["Expense"] != DBNull.Value ? Convert.ToDecimal(rdr["Expense"]) : 0
                            });
                        }
                    }

                    conn.Close();
                }

                return new ResponseResult<MonthlyIncomeExpense>
                {
                    Code = ResponseResultCode.Success,
                    Values = result.ToArray()
                };
            }
            catch (Exception ex)
            {
                return new ResponseResult<MonthlyIncomeExpense>
                {
                    Code = ResponseResultCode.Failed,
                    Message = ex.Message
                };
            }
        }

        public ResponseResult<DailyExpenseAverage> GetAverageDailyExpense(string userId, int? days)
        {
            try
            {
                var result = new DailyExpenseAverage();

                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    var cmd = new SqlCommand("GetAverageDailyExpense", conn)
                    {
                        CommandType = CommandType.StoredProcedure,
                        CommandTimeout = 90
                    };

                    cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                    cmd.Parameters.AddWithValue("@Days", days);

                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            result = new DailyExpenseAverage
                            {
                                AverageDailyExpense = rdr["AverageDailyExpense"] != DBNull.Value
                                    ? Convert.ToDecimal(rdr["AverageDailyExpense"])
                                    : 0
                            };
                        }
                    }

                    conn.Close();
                }

                return new ResponseResult<DailyExpenseAverage>
                {
                    Code = ResponseResultCode.Success,
                    Value = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseResult<DailyExpenseAverage>
                {
                    Code = ResponseResultCode.Failed,
                    Message = ex.Message
                };
            }
        }

        public ResponseResult<DailyExpense> GetDailyExpenses(string userId, int? days)
        {
            try
            {
                var result = new List<DailyExpense>();

                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    var cmd = new SqlCommand("GetDailyExpenses", conn)
                    {
                        CommandType = CommandType.StoredProcedure,
                        CommandTimeout = 90
                    };

                    cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                    cmd.Parameters.AddWithValue("@Days", days);

                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            result.Add(new DailyExpense
                            {
                                Date = Convert.ToDateTime(rdr["Date"]),
                                Total = rdr["Total"] != DBNull.Value ? Convert.ToDecimal(rdr["Total"]) : 0
                            });
                        }
                    }

                    conn.Close();
                }

                return new ResponseResult<DailyExpense>
                {
                    Code = ResponseResultCode.Success,
                    Values = result.ToArray()
                };
            }
            catch (Exception ex)
            {
                return new ResponseResult<DailyExpense>
                {
                    Code = ResponseResultCode.Failed,
                    Message = ex.Message
                };
            }
        }

        public ResponseResult<TransactionDTO> GetLastTransactions(string userId, int? count = 20, int? days = 30)
        {
            try
            {
                var result = new List<TransactionDTO>();

                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    var cmd = new SqlCommand("GetLastTransactions", conn)
                    {
                        CommandType = CommandType.StoredProcedure,
                        CommandTimeout = 90
                    };

                    cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                    cmd.Parameters.Add(new SqlParameter("@Count", count));
                    cmd.Parameters.AddWithValue("@Days", days);

                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            result.Add(new TransactionDTO
                            {
                                Id = Guid.Parse(rdr["Id"]?.ToString()),
                                UserId = Guid.Parse(rdr["UserId"]?.ToString()),
                                CategoryId = int.Parse(rdr["CategoryId"]?.ToString()),
                                CurrencyId = int.Parse(rdr["CurrencyId"]?.ToString()),
                                Amount = rdr["Amount"] != DBNull.Value ? Convert.ToDecimal(rdr["Amount"]) : 0,
                                Date = Convert.ToDateTime(rdr["Date"]),
                                Notes = rdr["Notes"]?.ToString()
                            });
                        }
                    }

                    conn.Close();
                }

                return new ResponseResult<TransactionDTO>
                {
                    Code = ResponseResultCode.Success,
                    Values = result.ToArray()
                };
            }
            catch (Exception ex)
            {
                return new ResponseResult<TransactionDTO>
                {
                    Code = ResponseResultCode.Failed,
                    Message = ex.Message
                };
            }
        }
    }
}
