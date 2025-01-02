using FinPlanner360.Business.Interfaces.Repositories;
using FinPlanner360.Business.Interfaces.Services;
using FinPlanner360.Business.Models;
using FinPlanner360.Repositories.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FinPlanner360.Repositories.Repositories;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(FinPlanner360DbContext context, IAppIdentityUser appIdentityUser)
        : base(context, appIdentityUser)
    {
    }

    public Guid? UserId
    {
        get
        {
            return _appIdentityUser != null ? _appIdentityUser.GetUserId() : null;
        }
    }

    public override async Task<ICollection<Category>> GetAllAsync() =>
        await _dbSet.Where(x => (x.UserId == null || x.UserId == UserId.Value) && x.RemovedDate == null).OrderBy(x => x.Description).ToListAsync();


    public async Task<Category> GetCategoryById(Guid id)
    {
        Guid? userId = _appIdentityUser != null ? _appIdentityUser.GetUserId() : null;

        return await _context.Categories
            .AsNoTracking()
            .Where(c => c.CategoryId == id && (c.UserId == null || c.UserId == UserId.Value))
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