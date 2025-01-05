using System.ComponentModel.DataAnnotations;

namespace FinPlanner360.Api.ViewModels.Budget;

public class BudgetUpdateViewModel
{
    [Required(ErrorMessage = "A código do orçamento é obrigatória.")]
    public Guid BudgetId { get; set; }

    [Required(ErrorMessage = "A categoria é obrigatória.")]
    public Guid CategoryId { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
    public decimal Amount { get; set; }
}