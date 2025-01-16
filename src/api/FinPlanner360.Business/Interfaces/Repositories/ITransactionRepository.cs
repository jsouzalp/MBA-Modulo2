using FinPlanner360.Business.Models;

namespace FinPlanner360.Business.Interfaces.Repositories;

public interface ITransactionRepository : IRepository<Transaction>
{
    Task<ICollection<Transaction>> GetBalanceByMonthYearAsync(DateTime date);

    Task<decimal> GetBalanceByMonthYearAndCatregoryAsync(DateTime date, Guid categoryId);

    Task<decimal> GetWalletBalanceAsync(DateTime referenceDate);

    Task<ICollection<Transaction>> GetTransactionsByRangeAsync(DateTime startDate, DateTime endDate);
    
    Task<ICollection<Transaction>> GetTransactionsWithCategoryByRangeAsync(DateTime startDate, DateTime endDate);
}