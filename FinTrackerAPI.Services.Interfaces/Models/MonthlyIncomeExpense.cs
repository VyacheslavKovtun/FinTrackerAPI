namespace FinTrackerAPI.Services.Interfaces.Models
{
    public class MonthlyIncomeExpense
    {
        public string Month { get; set; }
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
    }
}
