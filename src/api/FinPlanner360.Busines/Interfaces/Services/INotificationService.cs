using FinPlanner360.Busines.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPlanner360.Busines.Interfaces.Services
{
    public interface INotificationService
    {
        bool HasNotification();
        ICollection<Notification> GetNotifications();
        void Handle(Notification notification);
    }
}
