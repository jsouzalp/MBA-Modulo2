using FinPlanner360.Busines.Interfaces.Services;
using FinPlanner360.Busines.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPlanner360.Busines.Services
{
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
}
