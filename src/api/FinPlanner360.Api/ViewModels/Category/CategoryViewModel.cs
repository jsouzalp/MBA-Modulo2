using FinPlanner360.Busines.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace FinPlanner360.Api.ViewModels.Category;

public class CategoryViewModel
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public Guid CategoryId { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public string Description { get; set; }

    public CategoryTypeEnum Type { get; set; }
}