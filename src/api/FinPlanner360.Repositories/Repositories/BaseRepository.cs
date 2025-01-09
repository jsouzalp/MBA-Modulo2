using FinPlanner360.Business.Interfaces.Repositories;
using FinPlanner360.Business.Interfaces.Services;
using FinPlanner360.Business.Models;
using FinPlanner360.Repositories.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FinPlanner360.Repositories.Repositories;

public abstract class BaseRepository<T> : IRepository<T> where T : Entity, new()
{
    protected readonly IAppIdentityUser _appIdentityUser;
    protected readonly FinPlanner360DbContext _context;
    protected readonly DbSet<T> _dbSet;

    protected BaseRepository(FinPlanner360DbContext context, IAppIdentityUser appIdentityUser)
    {
        _appIdentityUser = appIdentityUser;
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public void Dispose()
    {
        _context?.Dispose();
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<ICollection<T>> GetAllAsync()
    {
        Guid? userId = _appIdentityUser != null ? _appIdentityUser.GetUserId() : null;

        if (userId != null)
        {
            return await _dbSet.Where(x => x.UserId == userId.Value).ToListAsync();
        }
        else
        {
            return await _dbSet.ToListAsync();
        }
    }

    public async Task<ICollection<T>> FilterAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
    }

    public async Task CreateAsync(T entity)
    {
        _dbSet.Add(entity);
        _ = await SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        _ = await SaveChangesAsync();
    }

    public async Task RemoveAsync(Guid id)
    {
        T entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await SaveChangesAsync();
        }

    }

    public async Task RemoveAsync(T entity)
    {
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await SaveChangesAsync();
        }
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}