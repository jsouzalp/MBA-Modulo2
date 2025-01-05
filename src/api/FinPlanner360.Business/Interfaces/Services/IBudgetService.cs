using FinPlanner360.Business.Models;

namespace FinPlanner360.Business.Interfaces.Services;

public interface IBudgetService
{
    Task<ICollection<Budget>> GetAllAsync();

    Task CreateAsync(Budget budget);

    Task UpdateAsync(Budget budget);

    Task DeleteAsync(Guid budgetId);
}