using FinPlanner360.Busines.Interfaces.Validations;
using FinPlanner360.Business.Interfaces.Services;
using FluentValidation;

namespace FinPlanner360.Busines.Models.Validations
{
    public class ValidationFactory<TI> : IValidationFactory<TI> where TI : class
    {
        private readonly IValidator<TI> _validator;
        private readonly INotificationService _notificationService;

        public ValidationFactory(INotificationService notificationService,
            IValidator<TI> validator)
        {
            _notificationService = notificationService;
            _validator = validator;
        }

        public async Task<bool> ValidateAsync(TI input)
        {
            var resultValidation = await _validator.ValidateAsync(input);

            if (!resultValidation.IsValid)
            {
                foreach (var item in resultValidation.Errors)
                {
                    _notificationService.Handle(item.ErrorMessage);
                }
            }

            return resultValidation.IsValid;
        }
    }
}
