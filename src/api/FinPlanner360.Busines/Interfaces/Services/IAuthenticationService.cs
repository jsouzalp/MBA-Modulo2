
using FinPlanner360.Busines.Models;

namespace FinPlanner360.Busines.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<(Guid UserId, string AccessToken)> RegisterUserAsync(string email, string password);
        Task<string> LoginUserAsync(string email, string password);
    }
}
