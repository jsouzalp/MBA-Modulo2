using FinPlanner360.Busines.Models;

namespace FinPlanner360.Busines.Interfaces.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category> GetCategoryById(Guid id);
}