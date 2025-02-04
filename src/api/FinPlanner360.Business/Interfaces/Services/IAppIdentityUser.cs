namespace FinPlanner360.Business.Interfaces.Services;

public interface IAppIdentityUser
{
    Guid GetUserId();

    bool IsAuthenticated();

    string GetUserEmail();
}