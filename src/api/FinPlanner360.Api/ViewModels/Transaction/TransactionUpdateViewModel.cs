using System.ComponentModel.DataAnnotations;

namespace FinPlanner360.Api.ViewModels.Transaction;

public class TransactionUpdateViewModel
{
    [Required(ErrorMessage = "A transação é obrigatória.")]
    public Guid TransactionId { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "A categoria é obrigatória.")]
    public Guid CategoryId { get; set; }
}