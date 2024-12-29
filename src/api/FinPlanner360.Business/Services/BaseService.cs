using FinPlanner360.Busines.Interfaces.Services;
using FinPlanner360.Busines.Models;
using FluentValidation;
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
        _notificationService.Handle(new Notification(message));
    }

    protected bool IsValid<TV, TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE> where TE : Entity
    {
        var validator = validacao.Validate(entidade);

        if (validator.IsValid) return true;

        Notify(validator);

        return false;
    }
}