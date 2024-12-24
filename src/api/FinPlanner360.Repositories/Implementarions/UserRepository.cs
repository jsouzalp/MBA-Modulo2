using FinPlanner360.Entities.Users;
using FinPlanner360.Repositories.Abstractions;
using FinPlanner360.Repositories.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FinPlanner360.Repositories.Implementarions
{
    public class UserRepository : IUserRepository
    {
        private readonly FinPlanner360DbContext _context;
        public UserRepository(FinPlanner360DbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserId == id);
        }
    }
}
