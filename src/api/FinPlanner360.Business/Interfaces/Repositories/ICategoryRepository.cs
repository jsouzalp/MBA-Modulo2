using FinPlanner360.Business.Models;

namespace FinPlanner360.Business.Interfaces.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category> GetCategoryById(Guid id);
}