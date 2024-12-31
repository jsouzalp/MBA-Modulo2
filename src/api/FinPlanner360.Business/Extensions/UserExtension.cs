using FinPlanner360.Business.Models;

namespace FinPlanner360.Business.Extensions;

public static class UserExtension
{
    public static User FillAttributes(this User user)
    {
        if (user.UserId == Guid.Empty) { user.UserId = Guid.NewGuid(); }

        if (user.Transactions != null)
        {
            foreach (Transaction transaction in user.Transactions)
            {
                transaction.UserId = user.UserId;
            }
        }

        if (user.Categories != null)
        {
            foreach (Category category in user.Categories)
            {
                category.UserId = user.UserId;
            }
        }

        if (user.Budgets != null)
        {
            foreach (Budget budget in user.Budgets)
            {
                budget.UserId = user.UserId;
            }
        }

        if (user.GeneralBudgets != null)
        {
            foreach (GeneralBudget generalBudget in user.GeneralBudgets)
            {
                generalBudget.UserId = user.UserId;
            }
        }

        return user;
    }
}