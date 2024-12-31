using FinPlanner360.Business.Models;

namespace FinPlanner360.Business.Extensions;

public static class CategoryExtension
{
    public static Category FillAttributes(this Category category)
    {
        if (category.CategoryId == Guid.Empty) { category.CategoryId = Guid.NewGuid(); }
        if (category.CreatedDate == DateTime.MinValue || category.CreatedDate == DateTime.MaxValue) { category.CreatedDate = DateTime.Now; }

        return category;
    }
}