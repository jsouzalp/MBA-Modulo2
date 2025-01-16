namespace FinPlanner360.Api.ViewModels.Dashboard;

public class CardSumaryViewModel
{
    public decimal WalletBalance { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal TotalBalance { get; set; }
    public decimal TotalIncomeToday { get; set; }
    public decimal TotalExpenseToday { get; set; }
    public decimal FutureTotalIncome { get; set; }
    public decimal FutureTotalExpense { get; set; }
}
