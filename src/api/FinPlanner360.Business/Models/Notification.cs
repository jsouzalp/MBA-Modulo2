using FinPlanner360.Business.Models.Enums;

namespace FinPlanner360.Business.Models;

public class Notification
{
    public Notification(string message)
    {
        Message = message;
        Type = NotificationTypeEnum.Error;
    }

    public Notification(string message, NotificationTypeEnum type)
    {
        Message = message;
        Type = type;
    }

    public string Message { get; }
    public NotificationTypeEnum Type { get; }
}