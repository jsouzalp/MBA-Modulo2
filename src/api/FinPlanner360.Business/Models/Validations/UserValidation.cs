using FinPlanner360.Business.Models;
using FluentValidation;

namespace FinPlanner360.Busines.Models.Validations;

public class UserValidation : AbstractValidator<User>
{
    public UserValidation()
    {
        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty)
            .WithMessage("ID do usuário não foi preenchido");

        RuleFor(x => x.Name)
            .NotEqual(string.Empty)
            .WithMessage("Nome do usuário não pode ser vazio")
            .NotEmpty()
            .WithMessage("Nome do usuário não pode ser nulo")
            .MinimumLength(3)
            .WithMessage("Nome do usuário deve ter ao menos 3 caracteres")
            .MaximumLength(50)
            .WithMessage("Nome do usuário deve ter no máximo 50 caracteres");

        RuleFor(x => x.Email)
            .NotEqual(string.Empty)
            .WithMessage("Email não pode ser vazio")
            .NotEmpty()
            .WithMessage("Email não pode ser nulo")
            .EmailAddress()
            .WithMessage("Email informado está em um formato errado");

        RuleFor(x => x.AuthenticationId)
            .NotEqual(Guid.Empty)
            .WithMessage("ID de autenticação não foi preenchido");
    }
}