using FinPlanner360.Business.Models.Enums;

namespace FinPlanner360.Api.ViewModels.Dashboard;

public class TransactionYearEvolutionViewModel
{
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal TotalBalance { get; set; }
}
