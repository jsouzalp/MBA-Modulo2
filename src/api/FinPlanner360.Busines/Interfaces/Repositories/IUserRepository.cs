
using FinPlanner360.Busines.Models;

namespace FinPlanner360.Busines.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
    }
}
