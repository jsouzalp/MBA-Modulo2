using FinPlanner360.Business.Interfaces.Repositories;
using FinPlanner360.Business.Interfaces.Services;
using FinPlanner360.Business.Models;
using FinPlanner360.Repositories.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FinPlanner360.Repositories.Repositories;

public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(FinPlanner360DbContext context, IAppIdentityUser appIdentityUser)
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

    public override async Task<ICollection<Transaction>> GetAllAsync()
    {
        return await _dbSet
            .Include(x => x.Category)
            .Where(x => x.UserId == UserId)
            .ToListAsync();
    }
}