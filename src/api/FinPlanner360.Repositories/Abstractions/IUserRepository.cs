using FinPlanner360.Entities.Users;

namespace FinPlanner360.Repositories.Abstractions
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(Guid id);
    }
}
