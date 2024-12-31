using FinPlanner360.Business.Interfaces.Services;
using FinPlanner360.Business.Models;

namespace FinPlanner360.Business.Services;

public class BaseService
{
    private readonly INotificationService _notificationService;

    public BaseService(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    //protected void Notificar(ValidationResult validationResult)
    //{
    //    foreach (var error in validationResult.Errors)
    //    {
    //        Notificar(error.ErrorMessage);
    //    }
    //}

    protected void Notify(string message)
    {
        _notificationService.Handle(new Notification(message));
    }

    //protected bool ExecutarValidacao<TV, TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE> where TE : Entity
    //{
    //    var validator = validacao.Validate(entidade);

    //    if (validator.IsValid) return true;

    //    Notificar(validator);

    //    return false;
    //}
}