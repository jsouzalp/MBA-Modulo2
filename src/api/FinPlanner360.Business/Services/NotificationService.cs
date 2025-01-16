using FinPlanner360.Business.Interfaces.Services;
using FinPlanner360.Business.Models;
using FinPlanner360.Business.Models.Enums;

namespace FinPlanner360.Business.Services;

public class NotificationService : INotificationService
{
    private ICollection<Notification> _notification;

    public NotificationService()
    {
        _notification = [];
    }

    public bool HasNotification() => _notification != null && _notification.Any();

    public ICollection<Notification> GetNotifications() => _notification;
    public bool HasError() => _notification.Any(j => j.Type == NotificationTypeEnum.Error);

    public void Handle(Notification notification)
    {
        _notification.Add(notification);
    }

    public void Handle(string notification)
    {
        Handle(new Notification(notification, NotificationTypeEnum.Error));
    }
}