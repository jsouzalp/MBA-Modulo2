using FinPlanner360.Business.Models.Enums;

namespace FinPlanner360.Business.DTO.Transacton;

public class BalanceDTO
{
    public Guid TransactionId { get; set; }

    public string Description { get; set; }

    public decimal Amount { get; set; }
    public TransactionTypeEnum Type { get; set; }

    public Guid CategoryId { get; set; }

    public string Category { get; set; }

    public DateTime TransactionDate { get; set; }

    public decimal CategoryBalance { get; set; }
}