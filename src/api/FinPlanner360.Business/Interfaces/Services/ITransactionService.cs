using FinPlanner360.Business.DTO.Transacton;
using FinPlanner360.Business.Models;

namespace FinPlanner360.Business.Interfaces.Services;

public interface ITransactionService
{
    Task<ICollection<BalanceDTO>> GetBalanceByMonthYearAsync(DateTime date);

    Task<Transaction> GetTransactionByIdAsync(Guid id);

    Task CreateAsync(Transaction transaction);

    Task UpdateAsync(Transaction transactionUpdate);

    Task DeleteAsync(Guid TransactionId);
}