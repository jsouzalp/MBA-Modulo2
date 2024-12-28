using FinPlanner360.Busines.Interfaces.Repositories;
using FinPlanner360.Busines.Models;
using FinPlanner360.Repositories.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FinPlanner360.Repositories.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(FinPlanner360DbContext context)
        : base(context)
    {
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
    }
}