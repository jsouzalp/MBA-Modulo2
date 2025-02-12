using FinPlanner360.Business.Interfaces.Repositories;
using FinPlanner360.Business.Interfaces.Services;
using FinPlanner360.Business.Models;
using FinPlanner360.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FinPlanner360.Data.Repositories;

public class BudgetRepository : BaseRepository<Budget>, IBudgetRepository
{
    public BudgetRepository(FinPlanner360DbContext context, IAppIdentityUser appIdentityUser)
        : base(context, appIdentityUser)
    {
    }

    public override async Task<ICollection<Budget>> GetAllAsync()
    {
        Guid? userId = _appIdentityUser != null ? _appIdentityUser.GetUserId() : null;

        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Category)
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }


    public async Task<Budget> GetBudgetByCategoryId(Guid id)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(c => c.CategoryId == id && c.UserId == UserId)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ExistsAsync()
    {
        return await _dbSet
            .AsNoTracking()
            .Where(c => c.UserId == UserId)
            .AnyAsync();
    }
}