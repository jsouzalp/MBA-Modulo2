using FinPlanner360.Busines.Interfaces.Services;
using FinPlanner360.Busines.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPlanner360.Busines.Services
{
    public class NotificationService : INotificationService
    {
        private ICollection<Notification> _notification;

        public NotificationService()
        {
            _notification = [];
        }

        public bool HasNotification() => _notification != null && _notification.Any();
        public ICollection<Notification> GetNotifications() => _notification;

        public void Handle(Notification notification)
        {
            _notification.Add(notification);
        }
    }
}
