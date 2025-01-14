using FinPlanner360.Business.Models.Enums;

namespace FinPlanner360.Api.ViewModels.Report
{
    public class TransactionCategoyViewModel
    {
        public string CategoryDescription { get; set; }
        public decimal TotalAmount { get; set; }
        public TransactionTypeEnum Type { get; set; }
    }
}
