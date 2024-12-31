using FinPlanner360.Business.Models;

namespace FinPlanner360.Business.Extensions;

public static class BudgetExtension
{
    public static Budget FillAttributes(this Budget budget)
    {
        if (budget.BudgetId == Guid.Empty) { budget.BudgetId = Guid.NewGuid(); }
        if (budget.CreatedDate == DateTime.MinValue || budget.CreatedDate == DateTime.MaxValue) { budget.CreatedDate = DateTime.Now; }

        return budget;
    }
}