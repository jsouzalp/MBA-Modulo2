using FinPlanner360.Business.Models.Enums;

namespace FinPlanner360.Api.ViewModels.Report
{
    public class TransactionAnalyticsViewModel
    {
        public string CategoryDescription { get; set; }
        public string TransactionDate { get; set; }
        public string Description { get; set; }
        public string TotalAmount { get; set; }
        public string Type { get; set; }
    }
}
