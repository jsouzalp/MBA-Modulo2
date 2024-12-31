using FinPlanner360.Business.Models;

namespace FinPlanner360.Business.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetByEmailAsync(string email);
}