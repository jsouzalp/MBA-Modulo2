using FinPlanner360.Business.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace FinPlanner360.Api.ViewModels.Transaction;

public class TransactionViewModel
{
    
    public Guid? TransactionId { get; set; }
    
    [Required(ErrorMessage = "A descrição é obrigatória.")]
    public string Description { get; set; }
    
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
    public decimal Amount { get; set; }
    public CategoryTypeEnum Type { get; set; }
    
    [Required(ErrorMessage = "A categoria é obrigatória.")]
    public Guid CategoryId { get; set; }
    
    public DateTime? TransactionDate { get; set; }

    [Required(ErrorMessage = "O código do usuário é obrigatório.")]
    public Guid UserId { get; set; }


}