using FinPlanner360.Business.Models.Enums;

namespace FinPlanner360.Business.DTO.Transacton
{
    public class TransactionReportDTO
    {
        public CategoryTypeEnum Type { get; set; }
        public decimal TotalAmount { get; set; }
        public int TransactionCount { get; set; }
    }

}
