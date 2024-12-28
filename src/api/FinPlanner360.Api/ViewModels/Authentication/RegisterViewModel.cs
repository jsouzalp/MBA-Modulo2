using System.ComponentModel.DataAnnotations;

namespace FinPlanner360.Api.ViewModels.Authentication
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Atributo {0} é obrigatório")]
        [StringLength(maximumLength: 50, ErrorMessage = "Atributo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Atributo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "Atributo {0} está em formato inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Atributo {0} é obrigatório")]
        [StringLength(maximumLength: 25, ErrorMessage = "Atributo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 8)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "As senhas não conferem.")]
        public string ConfirmPassword { get; set; }
    }
}
