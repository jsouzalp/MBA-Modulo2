using FinPlanner360.Business.DTO.Transacton;
using FinPlanner360.Business.Interfaces.Repositories;
using FinPlanner360.Business.Interfaces.Services;

namespace FinPlanner360.Business.Services
{
    public class TransactionReportService : ITransactionReportService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionReportService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<IEnumerable<TransactionReportDTO>> GetTransactionReportByTypeAsync(DateTime startDate, DateTime endDate)
        {
            var reportDto = await _transactionRepository.GetTransactionReportByTypeAsync(startDate, endDate);
            
            return reportDto;
        }
    }

}
