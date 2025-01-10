using FinPlanner360.Business.Models.Enums;

namespace FinPlanner360.Api.ViewModels.Transaction;

public class TransactionListViewModel
{
    public Guid TransactionId { get; set; }

    public string Description { get; set; }

    public decimal Amount { get; set; }
    public TransactionTypeEnum Type { get; set; }

    public Guid CategoryId { get; set; }

    public DateTime TransactionDate { get; set; }

    public Guid UserId { get; set; }
    public decimal CategoryBalance { get; set; }
}