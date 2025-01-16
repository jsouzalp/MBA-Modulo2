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

    public async Task<ICollection<Transaction>> GetBalanceByMonthYearAsync(DateTime date)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Category)
            .Where(x => x.UserId == UserId &&
                    x.TransactionDate.Year == date.Year &&
                    x.TransactionDate.Month == date.Month)
            .OrderBy(x => x.TransactionDate)
            .ToListAsync();
    }

    public async Task<decimal> GetBalanceByMonthYearAndCatregoryAsync(DateTime date, Guid categoryId)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Category)
            .Where(x => x.UserId == UserId &&
                    x.CategoryId == categoryId &&
                    x.TransactionDate.Year == date.Year &&
                    x.TransactionDate.Month == date.Month)
            .SumAsync(x => x.Amount);
    }
}