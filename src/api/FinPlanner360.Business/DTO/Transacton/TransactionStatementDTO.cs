namespace FinPlanner360.Business.DTO.Transacton
{
    public class TransactionStatementDTO
    {
        public Guid TransactionId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
