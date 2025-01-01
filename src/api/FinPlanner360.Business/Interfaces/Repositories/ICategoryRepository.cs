using FinPlanner360.Business.Interfaces.Repositories;
using FinPlanner360.Business.Models;

namespace FinPlanner360.Busines.Interfaces.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category> GetCategoryById(Guid id);
}