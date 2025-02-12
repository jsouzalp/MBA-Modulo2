using FinPlanner360.Busines.Interfaces.Validations;
using FinPlanner360.Busines.Services;
using FinPlanner360.Business.Extensions;
using FinPlanner360.Business.Interfaces.Repositories;
using FinPlanner360.Business.Interfaces.Services;
using FinPlanner360.Business.Models;
using FinPlanner360.Data.Extensions;

namespace FinPlanner360.Business.Services;

public class GeneralBudgetService : BaseService, IGeneralBudgetService
{
    private readonly IGeneralBudgetRepository _budgetRepository;
    private readonly IValidationFactory<GeneralBudget> _validationFactory;

    public GeneralBudgetService(IValidationFactory<GeneralBudget> validationFactory,
        INotificationService notificationService,
        IGeneralBudgetRepository budgetRepository) : base(notificationService)
    {
        _validationFactory = validationFactory;
        _budgetRepository = budgetRepository;
    }

    public async Task CreateAsync(GeneralBudget budget)
    {
        if (!await _validationFactory.ValidateAsync(budget))
            return;

        if (!await BudgetExists())
        {
            await _budgetRepository.CreateAsync(budget.FillAttributes());
        }
    }

    public async Task UpdateAsync(GeneralBudget budgetUpdate)
    {
        var budget = await _budgetRepository.GetByIdAsync(budgetUpdate.GeneralBudgetId);
        budget.Amount = budgetUpdate.Amount;
        budget.Percentage = budgetUpdate.Percentage;

        if (!await _validationFactory.ValidateAsync(budget))
            return;

        await _budgetRepository.UpdateAsync(budget);

    }

    public async Task DeleteAsync(Guid budgetId)
    {
        var budget = await _budgetRepository.GetByIdAsync(budgetId);
        if (budget == null)
        {
            Notify("Previsão orçamentária não existe!");
            return;
        }

        await _budgetRepository.RemoveAsync(budget);
    }

    private async Task<bool> BudgetExists()
    {

        var budget = await _budgetRepository.GetAllAsync();

        if (budget.Count != 0)
        {
            Notify("Já existe um orçamento geral registrado.");
            return true;
        }

        return false;
    }
}