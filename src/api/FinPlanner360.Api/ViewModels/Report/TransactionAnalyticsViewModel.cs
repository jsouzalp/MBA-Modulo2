using FinPlanner360.Business.Models.Enums;

namespace FinPlanner360.Api.ViewModels.Report
{
    public class TransactionAnalyticsViewModel
    {
        public DateTime? TransactionDate { get; set; }
        public string CategoryDescription { get; set; }
        public decimal TotalAmount { get; set; }
        public TransactionTypeEnum Type { get; set; }
    }
}
