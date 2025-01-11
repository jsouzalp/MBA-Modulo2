using FinPlanner360.Business.Models;

namespace FinPlanner360.Business.Interfaces.Repositories;

public interface ITransaction_Repository : IRepository<Transaction>
{
    Task<ICollection<Transaction>> GetTransactionsByRangeAsync(DateTime startDate, DateTime endDate);
}