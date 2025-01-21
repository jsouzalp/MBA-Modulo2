using FinPlanner360.Business.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace FinPlanner360.Api.ViewModels.Report
{
    public class TransactionAnalyticsViewModel
    {
        [Display(Name = "Categoria")]
        public string CategoryDescription { get; set; }

        [Display(Name = "Data da Transação")]
        public string TransactionDate { get; set; }

        [Display(Name = "Descrição")]
        public string Description { get; set; }

        [Display(Name = "Valor Total")]
        public string TotalAmount { get; set; }

        [Display(Name = "Tipo")]
        public string Type { get; set; }

    }
}
