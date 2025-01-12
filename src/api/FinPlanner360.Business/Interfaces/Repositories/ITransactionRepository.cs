using FinPlanner360.Business.Models;

namespace FinPlanner360.Business.Interfaces.Repositories;

public interface ITransactionRepository : IRepository<Transaction>
{
    Task<ICollection<Transaction>> GetBalanceByMonthAsync(DateTime date);
}