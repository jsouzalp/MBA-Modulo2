
using FinPlanner360.Busines.Models;

namespace FinPlanner360.Busines.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<string> LoginUserAsync(string email, string password);
    }
}
