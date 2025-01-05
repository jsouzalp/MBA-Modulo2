using FinPlanner360.Business.Models;
using FluentValidation;

namespace FinPlanner360.Busines.Models.Validations;

public class GeneralBudgetValidation : AbstractValidator<GeneralBudget>
{
    public GeneralBudgetValidation()
    {
        RuleFor(c => c.UserId)
            .NotNull().WithMessage("O código do usuário precisa ser informado.")
            .NotEqual(Guid.Empty).WithMessage("O código do usuário precisa ser informado.");

        RuleFor(c => c)
            .Must(gb => (gb.Amount.HasValue ^ gb.Percentage.HasValue))
            .WithMessage("Você deve preencher apenas um campo: Valor ou Percentual, mas não ambos.");
    }
}