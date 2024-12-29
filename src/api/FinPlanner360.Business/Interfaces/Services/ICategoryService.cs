using FinPlanner360.Busines.Models;

namespace FinPlanner360.Busines.Interfaces.Services;

public interface ICategoryService
{
    Task CreateAsync(Category category);

    Task UpdateAsync(Category category);

    Task DeleteAsync(Guid categoryId);
}