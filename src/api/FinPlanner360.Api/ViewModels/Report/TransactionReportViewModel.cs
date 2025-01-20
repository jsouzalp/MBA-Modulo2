namespace FinPlanner360.Api.ViewModels.Report
{
    public class TransactionReportViewModel
    {
        public string Type { get; set; }
        public string CategoryDescription { get; set; }
        public decimal TotalAmount { get; set; }
        public int TransactionCount { get; set; }
    }

}
