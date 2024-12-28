namespace FinPlanner360.Busines.Interfaces.Services;

public interface IUserService
{
    Task<(Guid UserId, string AccessToken)> RegisterUserAsync(string email, string password);

    Task<string> LoginUserAsync(string email, string password);
}