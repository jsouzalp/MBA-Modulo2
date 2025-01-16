using FinPlanner360.Business.Models.Enums;

namespace FinPlanner360.Api.ViewModels.Transaction;

public class BalanceViewModel
{
    public Guid TransactionId { get; set; }

    public string Description { get; set; }

    public decimal Amount { get; set; }
    public CategoryTypeEnum Type { get; set; }

    public Guid CategoryId { get; set; }

    public string Category { get; set; }

    public DateTime TransactionDate { get; set; }

    public decimal CategoryBalance { get; set; }
}