namespace FinPlanner360.Business.Interfaces.Services;

public interface IAppIdentityUser
{
    //string GetUsername();
    Guid GetUserId();

    bool IsAuthenticated();

    string GetUserEmail();

    //bool IsInRole(string role);
    //string GetRemoteIpAddress();
    //string GetLocalIpAddress();
}