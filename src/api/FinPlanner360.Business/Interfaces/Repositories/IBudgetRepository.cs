using FinPlanner360.Business.Models;

namespace FinPlanner360.Business.Interfaces.Repositories;

public interface IBudgetRepository : IRepository<Budget>
{
    Task<Budget> GetBudgetByCategoryId(Guid id);
    Task<bool> ExistsAsync();
}