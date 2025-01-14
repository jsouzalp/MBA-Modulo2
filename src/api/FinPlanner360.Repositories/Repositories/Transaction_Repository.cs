using FinPlanner360.Business.Interfaces.Repositories;
using FinPlanner360.Business.Interfaces.Services;
using FinPlanner360.Business.Models;
using FinPlanner360.Repositories.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPlanner360.Repositories.Repositories;

public class Transaction_Repository : BaseRepository<Transaction>, ITransaction_Repository
{
    public Transaction_Repository(FinPlanner360DbContext context, IAppIdentityUser appIdentityUser)
    : base(context, appIdentityUser)
    {
    }

    public async Task<ICollection<Transaction>> GetTransactionsByRangeAsync(DateTime startDate, DateTime endDate)
    {
        if (!UserId.HasValue)
        {
            return null;
        }

        return await _dbSet
            .AsNoTracking()
            .Where(x => x.UserId == UserId.Value && x.TransactionDate >= startDate && x.TransactionDate <= endDate)
            .ToListAsync();
    }

    public async Task<ICollection<Transaction>> GetTransactionsWithCategoryByRangeAsync(DateTime startDate, DateTime endDate)
    {
        if (!UserId.HasValue)
        {
            return null;
        }

        //return await _dbSet.AsNoTracking()
        //    .Include(x => x.Category)
        //    .Where(x => x.UserId == UserId.Value && x.TransactionDate >= startDate && x.TransactionDate <= endDate)
        //    .ToListAsync();

        return await _dbSet.AsNoTracking()
            .Include(x => x.Category)
            .Where(x => x.UserId == UserId.Value)
            .ToListAsync();
    }
}
