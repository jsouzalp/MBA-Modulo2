using FinPlanner360.Busines.Interfaces.Repositories;
using FinPlanner360.Busines.Models;
using FinPlanner360.Repositories.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FinPlanner360.Repositories.Repositories;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(FinPlanner360DbContext context)
        : base(context)
    {
    }

    public async Task<Category> GetCategoryById(Guid id)
    {
        return await _context.Categories
            .AsNoTracking()
            .Where(c => c.CategoryId == id)
            .Select(c => new Category
            {
                CategoryId = c.CategoryId,
                Description = c.Description,
                Type = c.Type,
                UserId = c.UserId,
                Transactions = c.Transactions.Take(1).ToList() // Include only one Transaction
            })
            .FirstOrDefaultAsync();
    }
}