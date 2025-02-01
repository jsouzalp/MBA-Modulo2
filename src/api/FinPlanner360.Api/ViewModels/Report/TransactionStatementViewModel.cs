namespace FinPlanner360.Api.ViewModels.Report
{
    public class TransactionStatementViewModel
    {
        public Guid TransactionId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string FormattedAmount { get; set; }
        public string Category { get; set; }
        public string TransactionDate { get; set; }
    }
}
