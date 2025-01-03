using FinPlanner360.Busines.Services;
using FinPlanner360.Business.Interfaces.Services;

namespace FinPlanner360.Business.Services;

public class UserService : BaseService, IUserService
{
    public UserService(INotificationService notificationService) : base(notificationService)
    { }
}