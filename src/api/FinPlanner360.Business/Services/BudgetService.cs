using FinPlanner360.Busines.Interfaces.Validations;
using FinPlanner360.Busines.Services;
using FinPlanner360.Business.Extensions;
using FinPlanner360.Business.Interfaces.Repositories;
using FinPlanner360.Business.Interfaces.Services;
using FinPlanner360.Business.Models;

namespace FinPlanner360.Business.Services;

public class BudgetService : BaseService, IBudgetService
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly IValidationFactory<Budget> _validationFactory;

    public BudgetService(IValidationFactory<Budget> validationFactory,
        INotificationService notificationService,
        IBudgetRepository budgetRepository) : base(notificationService)
    {
        _validationFactory = validationFactory;
        _budgetRepository = budgetRepository;
    }
    public Task<ICollection<Budget>> GetAllAsync() => _budgetRepository.GetAllAsync();

    private async Task<bool> BudgetExists(Guid categoryId)
    {
        var budget = await _budgetRepository.FilterAsync(c => c.CategoryId == categoryId && c.RemovedDate == null);

        if (budget.Count != 0)
        {
            Notify("Já existe um Budget.");
            return true;
        }

        return false;
    }

    public async Task CreateAsync(Budget budget)
    {
        if (!await _validationFactory.ValidateAsync(budget))
            return;

        if (!await BudgetExists(budget.CategoryId))
        {
            await _budgetRepository.CreateAsync(budget.FillAttributes());
        }
    }

    public async Task UpdateAsync(Budget budget)
    {
        if (!await _validationFactory.ValidateAsync(budget))
            return;

        if (!await BudgetExists(budget.CategoryId))
        {
            await _budgetRepository.UpdateAsync(budget);
        }
    }

    public async Task DeleteAsync(Guid categoryId)
    {
        var category = await _budgetRepository.GetBudgetByCategoryId(categoryId);
        if (category == null)
        {
            Notify("Categoria não existe!");
            return;
        }

        await _budgetRepository.RemoveAsync(categoryId);
    }


}