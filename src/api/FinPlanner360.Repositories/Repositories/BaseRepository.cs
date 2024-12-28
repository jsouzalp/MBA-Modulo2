using FinPlanner360.Busines.Interfaces.Repositories;
using FinPlanner360.Busines.Models;
using FinPlanner360.Repositories.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinPlanner360.Repositories.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : Entity, new()
    {
        protected readonly FinPlanner360DbContext _context;
        protected readonly DbSet<T> _dbSet;

        protected BaseRepository(FinPlanner360DbContext context)
        {
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

        public async Task<ICollection<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
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
                entity.RemovedDate = DateTime.Now;
                await UpdateAsync(entity);
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
