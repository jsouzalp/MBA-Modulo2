using FinPlanner360.Business.Interfaces.Repositories;
using FinPlanner360.Business.Interfaces.Services;
using FinPlanner360.Business.Models;
using FinPlanner360.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FinPlanner360.Data.Repositories;

public class GeneralBudgetRepository : BaseRepository<GeneralBudget>, IGeneralBudgetRepository
{
    public GeneralBudgetRepository(FinPlanner360DbContext context, IAppIdentityUser appIdentityUser)
        : base(context, appIdentityUser)
    {
    }

    public override async Task<ICollection<GeneralBudget>> GetAllAsync()
    {
        Guid? userId = _appIdentityUser != null ? _appIdentityUser.GetUserId() : null;
        return await _dbSet
            .AsNoTracking()
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync()
    {
        Guid? userId = _appIdentityUser != null ? _appIdentityUser.GetUserId() : null;
        return await _dbSet
            .AsNoTracking()
            .Where(c => c.UserId == userId)
            .AnyAsync();
    }
}