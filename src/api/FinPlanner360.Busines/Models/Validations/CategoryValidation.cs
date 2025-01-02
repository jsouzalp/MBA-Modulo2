using FluentValidation;

namespace FinPlanner360.Busines.Models.Validations;

public class CategoryValidation : AbstractValidator<Category>
{
    public CategoryValidation()
    {
        RuleFor(c => c.Description)
            .NotEmpty().WithMessage("A descrição precisa ser informada.")
            .Length(4, 100).WithMessage("A descrição precisa ter entre {MinLength} e {MaxLength} caracteres.");

        RuleFor(c => c.Type)
            .IsInEnum().WithMessage("Tipo da categoria inválido.");

        RuleFor(c => c.UserId)
            .NotNull().WithMessage("O Id do usuário precisa ser informado.");
    }
}