using System.Text.Json.Serialization;

namespace FinPlanner360.Api.ViewModels.Report
{
    public class GroupedTransactionAnalyticsViewModel
    {
        public string CategoryDescription { get; set; }
        public List<TransactionAnalyticsViewModel> Transactions { get; set; }
    }
}
