using FinPlanner360.Business.Models.Enums;

namespace FinPlanner360.Api.ViewModels.Dashboard
{
    public class TransactionDashboardViewModel
    {
        public string CategoryDescription { get; set; }
        public decimal TotalAmount { get; set; }
        public CategoryTypeEnum Type { get; set; }
    }
}
