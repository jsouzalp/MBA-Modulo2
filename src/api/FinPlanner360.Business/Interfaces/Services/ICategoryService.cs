using FinPlanner360.Business.Models;

namespace FinPlanner360.Business.Interfaces.Services;

public interface ICategoryService
{
    Task CreateAsync(Category category);

    Task UpdateAsync(Category category);

    Task DeleteAsync(Guid categoryId);
}