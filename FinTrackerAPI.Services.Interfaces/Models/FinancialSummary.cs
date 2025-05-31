namespace FinTrackerAPI.Services.Interfaces.Models
{
    public class FinancialSummary
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal Savings { get; set; }
        public decimal SavingsRate { get; set; }
    }
}
