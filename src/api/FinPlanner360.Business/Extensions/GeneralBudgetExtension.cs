using FinPlanner360.Business.Models;

namespace FinPlanner360.Repositories.Extensions;

public static class GeneralBudgetExtension
{
    public static GeneralBudget FillAttributes(this GeneralBudget generalBudget)
    {
        if (generalBudget.GeneralBudgetId == Guid.Empty) { generalBudget.GeneralBudgetId = Guid.NewGuid(); }
        if (generalBudget.CreatedDate == DateTime.MinValue || generalBudget.CreatedDate == DateTime.MaxValue) { generalBudget.CreatedDate = DateTime.Now; }

        return generalBudget;
    }
}