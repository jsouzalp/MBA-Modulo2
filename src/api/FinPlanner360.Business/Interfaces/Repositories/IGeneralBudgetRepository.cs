using FinPlanner360.Business.Models;

namespace FinPlanner360.Business.Interfaces.Repositories;

public interface IGeneralBudgetRepository : IRepository<GeneralBudget>
{
    Task<bool> ExistsAsync();
}