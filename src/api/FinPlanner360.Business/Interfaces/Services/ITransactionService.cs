using FinPlanner360.Business.Models;

namespace FinPlanner360.Business.Interfaces.Services;

public interface ITransactionService
{
    Task<ICollection<Transaction>> GetAllAsync();

    Task<Transaction> GetTransactionByIdAsync(Guid id);

    Task CreateAsync(Transaction transaction);

    Task UpdateAsync(Transaction transactionUpdate);

    Task DeleteAsync(Guid TransactionId);
}