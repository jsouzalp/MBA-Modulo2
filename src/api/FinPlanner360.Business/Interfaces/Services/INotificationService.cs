using FinPlanner360.Business.Models;
using FinPlanner360.Business.Models.Enums;

namespace FinPlanner360.Business.Interfaces.Services;

public interface INotificationService
{
    bool HasNotification();
    bool HasError();

    ICollection<Notification> GetNotifications();

    void Handle(Notification notification);

    void Handle(string notification);
}