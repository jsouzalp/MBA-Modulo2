using FinPlanner360.Business.Models;

namespace FinPlanner360.Business.Interfaces.Services;

public interface IGeneralBudgetService
{
    Task CreateAsync(GeneralBudget budget);

    Task UpdateAsync(GeneralBudget budget);

    Task DeleteAsync(Guid budgetId);
}