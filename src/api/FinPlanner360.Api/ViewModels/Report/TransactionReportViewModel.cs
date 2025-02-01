namespace FinPlanner360.Api.ViewModels.Report
{
    public class TransactionReportViewModel
    {
        public string Type { get; set; }
        public decimal TotalAmount { get; set; }
        public string FormattedTotalAmount { get; set; }
        public int TransactionCount { get; set; }
    }

}
