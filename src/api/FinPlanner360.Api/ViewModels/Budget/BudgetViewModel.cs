using System.ComponentModel.DataAnnotations;

namespace FinPlanner360.Api.ViewModels.Budget;

public class BudgetViewModel
{
    public Guid BudgetId { get; set; }

    [Required(ErrorMessage = "A categoria é obrigatória.")]
    public Guid CategoryId { get; set; }
    public string Description { get; set; }

    [Required(ErrorMessage = "O código do usuário é obrigatório.")]
    public Guid UserId { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
    public decimal Amount { get; set; }
}