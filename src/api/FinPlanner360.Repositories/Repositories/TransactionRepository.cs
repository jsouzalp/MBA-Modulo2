using FinPlanner360.Business.DTO.Transacton;
using FinPlanner360.Business.Interfaces.Repositories;
using FinPlanner360.Business.Interfaces.Services;
using FinPlanner360.Business.Models;
using FinPlanner360.Business.Models.Enums;
using FinPlanner360.Repositories.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FinPlanner360.Repositories.Repositories;

public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(FinPlanner360DbContext context, IAppIdentityUser appIdentityUser)
        : base(context, appIdentityUser)
    { }

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

    public async Task<decimal> GetWalletBalanceAsync(DateTime referenceDate)
    {
        if (!UserId.HasValue)
        {
            return 0.00m;
        }

        // POG por causa do SQLite !!!!!
        return (decimal)await _dbSet
            .AsNoTracking()
            .Include(x => x.Category)
            .Where(x => x.UserId == UserId.Value && x.TransactionDate < referenceDate)
            .SumAsync(x => (x.Category.Type == CategoryTypeEnum.Expense ? ((double)x.Amount * -1) : (double)x.Amount));
    }

    public async Task<ICollection<Transaction>> GetTransactionsByRangeAsync(DateTime startDate, DateTime endDate)
    {
        if (!UserId.HasValue)
        {
            return null;
        }

        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Category)
            .Where(x => x.UserId == UserId.Value && x.TransactionDate >= startDate && x.TransactionDate <= endDate)
            .ToListAsync();
    }

    public async Task<ICollection<Transaction>> GetTransactionsWithCategoryByRangeAsync(DateTime startDate, DateTime endDate)
    {
        if (!UserId.HasValue)
        {
            return null;
        }

        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Category)
            .Where(x => x.UserId == UserId.Value && x.TransactionDate >= startDate && x.TransactionDate <= endDate)
        .ToListAsync();
    }

    public async Task<ICollection<TransactionReportDTO>> GetTransactionReportByTypeAsync(DateTime startDate, DateTime endDate) 
    { 
        if (!UserId.HasValue) 
        {
            return null; 
        } 
        
        var transactions = await _dbSet.AsNoTracking()
            .Include(x => x.Category)
            .Where(x => x.UserId == UserId.Value && x.TransactionDate >= startDate && x.TransactionDate <= endDate)
            .ToListAsync(); 
        
        var report = transactions
            .GroupBy(x => new { x.Category.Type })
            .Select(g => new TransactionReportDTO 
            { 
                Type = g.Key.Type,                 
                TotalAmount = g.Sum(x => x.Amount), 
                TransactionCount = g.Count() 
            })
            .ToList(); 
        
        return report; 
    }

    public async Task<ICollection<TransactionStatementDTO>> GetTransactionStatementAsync(DateTime startDate, DateTime endDate)
    {
        if (!UserId.HasValue)
        {
            return null;
        }

        var transactions = await _dbSet.AsNoTracking()
            .Include(x => x.Category)
            .Where(x => x.UserId == UserId.Value && x.TransactionDate >= startDate && x.TransactionDate <= endDate)
            .ToListAsync();

        var statement = transactions.Select(t => new TransactionStatementDTO
        {
            TransactionId = t.TransactionId,
            Description = t.Description,
            Amount = t.Amount,
            Category = t.Category.Description,
            TransactionDate = t.TransactionDate
        }).ToList();

        return statement;
    }


}