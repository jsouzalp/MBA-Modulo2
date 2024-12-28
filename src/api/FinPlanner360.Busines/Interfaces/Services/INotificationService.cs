using FinPlanner360.Busines.Models;

namespace FinPlanner360.Busines.Interfaces.Services;

public interface INotificationService
{
    bool HasNotification();

    ICollection<Notification> GetNotifications();

    void Handle(Notification notification);
}