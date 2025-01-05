using FinPlanner360.Business.Models;
using System.Linq.Expressions;

namespace FinPlanner360.Business.Interfaces.Repositories;

public interface IRepository<TEntity> : IDisposable where TEntity : Entity
{
    Task<TEntity> GetByIdAsync(Guid id);

    Task<ICollection<TEntity>> GetAllAsync();

    Task<ICollection<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> predicate);

    Task CreateAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    Task RemoveAsync(Guid id);
    Task RemoveAsync(TEntity entity);

    Task<int> SaveChangesAsync();
}