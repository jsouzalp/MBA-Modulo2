using FinPlanner360.Business.Models;
using FluentValidation;

namespace FinPlanner360.Busines.Models.Validations;

public class BudgetValidation : AbstractValidator<Budget>
{
    public BudgetValidation()
    {
        RuleFor(c => c.UserId)
            .NotNull().WithMessage("O código do usuário precisa ser informado.")
            .NotEqual(Guid.Empty).WithMessage("O código do usuário precisa ser informado.");

        RuleFor(c => c.CategoryId)
            .NotNull().WithMessage("A categoria é obrigatória.")
            .NotEqual(Guid.Empty).WithMessage("A categoria é obrigatória.");

        RuleFor(b => b.Amount)
            .GreaterThan(0)
            .WithMessage("O valor deve ser maior que zero.");
    }
}