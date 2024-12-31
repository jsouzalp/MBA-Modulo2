using FinPlanner360.Business.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace FinPlanner360.Api.ViewModels.Category;

public class CategoryViewModel
{
    [Required(ErrorMessage = "O código da categoria é obrigatório")]
    public Guid CategoryId { get; set; }

    [Required(ErrorMessage = "O código do usuário é obrigatório")]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "O campo descrição é obrigatório")]
    public string Description { get; set; }

    public CategoryTypeEnum Type { get; set; }
}