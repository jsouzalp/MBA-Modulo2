namespace FinPlanner360.Business.Interfaces.Services;

public interface IUserService
{
    Task<(Guid UserId, string AccessToken)> RegisterUserAsync(string email, string password);

    Task<string> LoginUserAsync(string email, string password);
}