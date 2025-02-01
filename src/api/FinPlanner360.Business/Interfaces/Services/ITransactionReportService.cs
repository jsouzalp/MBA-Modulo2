using FinPlanner360.Business.DTO.Transacton;

namespace FinPlanner360.Business.Interfaces.Services
{
    public interface ITransactionReportService
    {
        Task<IEnumerable<TransactionReportDTO>> GetTransactionReportByTypeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<TransactionStatementDTO>> GetTransactionStatementAsync(DateTime startDate, DateTime endDate);

    }

}
