using FinPlanner360.Business.Interfaces.Services;
using FinPlanner360.Business.Models;
using FinPlanner360.Business.Models.Enums;
using FluentValidation.Results;

namespace FinPlanner360.Busines.Services;

public class BaseService
{
    private readonly INotificationService _notificationService;

    public BaseService(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    protected void Notify(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
        {
            Notify(error.ErrorMessage);
        }
    }

    protected void Notify(string message)
    {
        _notificationService.Handle(new Notification(message, NotificationTypeEnum.Error));
    }

    protected void Notify(string message, NotificationTypeEnum type)
    {
        _notificationService.Handle(new Notification(message, type));
    }
}