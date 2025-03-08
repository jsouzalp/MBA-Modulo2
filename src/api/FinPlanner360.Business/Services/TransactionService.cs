using AutoMapper;
using FinPlanner360.Busines.Interfaces.Validations;
using FinPlanner360.Busines.Services;
using FinPlanner360.Business.DTO.Transacton;
using FinPlanner360.Business.Exceptions;
using FinPlanner360.Business.Extensions;
using FinPlanner360.Business.Interfaces.Repositories;
using FinPlanner360.Business.Interfaces.Services;
using FinPlanner360.Business.Models;
using FinPlanner360.Business.Models.Enums;
using Transaction = FinPlanner360.Business.Models.Transaction;

namespace FinPlanner360.Business.Services;

public class TransactionService : BaseService, ITransactionService
{
    private readonly IMapper _mapper;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IBudgetRepository _budgetRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IGeneralBudgetRepository _generalBudgetRepository;
    private readonly IValidationFactory<Transaction> _validationFactory;

    public TransactionService(IMapper mapper,
        IValidationFactory<Transaction> validationFactory,
        INotificationService notificationService,
        ITransactionRepository transactionRepository,
        IBudgetRepository budgetRepository,
        ICategoryRepository categoryRepository,
        IGeneralBudgetRepository generalbudgetRepository) : base(notificationService)
    {
        _mapper = mapper;
        _validationFactory = validationFactory;
        _transactionRepository = transactionRepository;
        _budgetRepository = budgetRepository;
        _categoryRepository = categoryRepository;
        _generalBudgetRepository = generalbudgetRepository;
    }

    public async Task<ICollection<BalanceDTO>> GetBalanceByMonthYearAsync(DateTime date)
    {
        ICollection<Budget> categoryBudget = [];
        var generalBudget = (await _generalBudgetRepository.GetAllAsync()).FirstOrDefault();

        if (generalBudget == null)
            categoryBudget = await _budgetRepository.GetAllAsync();

        var balances = _mapper.Map<ICollection<BalanceDTO>>(await _transactionRepository.GetBalanceByMonthYearAsync(date));

        var categorySummary = balances
            .GroupBy(b => b.CategoryId)
            .Select(group => new
            {
                CategoryId = group.Key,
                TotalAmount = group.Sum(b => b.Amount)
            })
            .ToList();

        foreach (var balance in balances)
        {
            // Convert expenses to negative amounts
            if (balance.Type == CategoryTypeEnum.Expense)
                balance.Amount = -balance.Amount;

            if (generalBudget == null)
            {
                // Get budget for the current balance's category
                var budget = categoryBudget.FirstOrDefault(j => j.CategoryId == balance.CategoryId)?.Amount ?? 0;
                //generalBudget

                // Get total spent for the current balance's category
                var spent = categorySummary.FirstOrDefault(s => s.CategoryId == balance.CategoryId)?.TotalAmount ?? 0;

                balance.CategoryBalance = budget - spent;
            }
        }

        return balances;
    }

    public async Task<Transaction> GetTransactionByIdAsync(Guid id) => await _transactionRepository.GetByIdAsync(id);

    public async Task CreateAsync(Transaction transaction)
    {
        if (!await _validationFactory.ValidateAsync(transaction))
            return;

        if (await BudgetOkAsync(transaction, 0.00m))
            await _transactionRepository.CreateAsync(transaction.FillAttributes());
    }

    public async Task UpdateAsync(Transaction transactionUpdate)
    {
        var transaction = await _transactionRepository.GetByIdAsync(transactionUpdate.TransactionId);
        decimal originalAmount = transaction.Amount;
        transaction.Amount = transactionUpdate.Amount;
        transaction.CategoryId = transactionUpdate.CategoryId;
        transaction.Description = transactionUpdate.Description;
        transaction.TransactionDate = transactionUpdate.TransactionDate;

        if (!await _validationFactory.ValidateAsync(transaction))
            return;

        if (await BudgetOkAsync(transaction, originalAmount))
            await _transactionRepository.UpdateAsync(transaction.FillAttributes());
    }

    public async Task DeleteAsync(Guid transactionId)
    {
        var budget = await _transactionRepository.GetByIdAsync(transactionId);
        if (budget == null)
        {
            Notify("Transação não existe!");
            return;
        }

        await _transactionRepository.RemoveAsync(transactionId);
    }

    private async Task<bool> BudgetOkAsync(Transaction transaction, decimal originalAmount)
    {
        if (await ItIsIncome(transaction))
            return true;

        var generalBudget = (await _generalBudgetRepository.GetAllAsync()).FirstOrDefault();
        if (generalBudget != null && (generalBudget.Percentage.HasValue || generalBudget.Amount.HasValue))
            return await GeneralBudgetOkAsync(transaction, generalBudget, originalAmount);

        var categoryBudget = await _budgetRepository.GetBudgetByCategoryId(transaction.CategoryId);
        if (categoryBudget != null)
            return await CategoryBudgetOkAsync(transaction, categoryBudget, originalAmount);

        return await BudgetOnRevenuesOkAsync(transaction, originalAmount);
    }

    private async Task<bool> ItIsIncome(Transaction transaction)
    {
        var category = await _categoryRepository.GetCategoryById(transaction.CategoryId) ?? throw new BusinessException("Categoria do lançamento não identificada, recarregue e tente novamente.");

        return category.Type == CategoryTypeEnum.Income;
    }

    private async Task<bool> GeneralBudgetOkAsync(Transaction transaction, GeneralBudget generalBudget, decimal originalAmount)
    {
        var balance = await _transactionRepository.GetBalanceByMonthYearAsync(transaction.TransactionDate);
        
        decimal usedBudget = balance.Where(j => j.Category.Type == CategoryTypeEnum.Expense).Sum(j => j.Amount) - originalAmount;
        decimal incomingBalance = balance.Where(j => j.Category.Type == CategoryTypeEnum.Income).Sum(j => j.Amount);

        // Determine the budget amount
        decimal budgetAmount = generalBudget.Amount.GetValueOrDefault() > 0
            ? generalBudget.Amount.Value
            : (incomingBalance * (generalBudget.Percentage.GetValueOrDefault() / 100m));

        // Calculate the used budget percentage
        decimal usedPercentage = ((usedBudget + transaction.Amount) / budgetAmount) * 100;

        if (usedPercentage > 100)
        {
            Notify("O lançamento não pode ser realizado, pois está acima do limite estabelecido pelo orçamento geral.");
            return false;
        }

        if (usedPercentage > 80 && usedPercentage < 100)
            Notify("O saldo está acima de 80% do seu orçamento geral planejado.", NotificationTypeEnum.Warning);
        else if (usedPercentage == 100)
            Notify("O saldo atingiu 100% seu orçamento geral planejado.", NotificationTypeEnum.Warning);

        return true;
    }

    private async Task<bool> CategoryBudgetOkAsync(Transaction transaction, Budget categoryBudget, decimal originalAmount)
    {
        var balance = await _transactionRepository.GetBalanceByMonthYearAsync(transaction.TransactionDate);

        decimal usedBudgetGeneral = balance.Where(j => j.Category.Type == CategoryTypeEnum.Expense).Sum(j => j.Amount) - originalAmount;
        decimal incomingBalance = balance.Where(j => j.Category.Type == CategoryTypeEnum.Income).Sum(j => j.Amount);

        decimal usedPercentageGeneral = ((usedBudgetGeneral + transaction.Amount) / incomingBalance) * 100;
        
        if (usedPercentageGeneral > 100)
        {
            Notify($"O lançamento não pode ser realizado, pois não existem receitas para cobrir o lançamento.");
            return false;
        }

        decimal usedBudgetFromCategory = balance.Where(j => j.Category.Type == CategoryTypeEnum.Expense && j.CategoryId == transaction.CategoryId).Sum(j => j.Amount) - originalAmount;

        // Calculate the used budget percentage
        decimal usedPercentage = ((usedBudgetFromCategory + transaction.Amount) / categoryBudget.Amount) * 100;
        
        if (usedPercentage > 100)
        {
            Notify($"O lançamento não pode ser realizado, pois está acima do limite estabelecido pelo orçamento da categoria.");
            return false;
        }

        if (usedPercentage > 80 && usedPercentage < 100)
            Notify("O saldo está acima de 80% do seu orçamento planejado.", NotificationTypeEnum.Warning);
        else if (usedPercentage == 100)
            Notify("O saldo dessa categoria atingiu 100% seu orçamento planejado.", NotificationTypeEnum.Warning);

        return true;
    }

    private async Task<bool> BudgetOnRevenuesOkAsync(Transaction transaction, decimal originalAmount)
    {
        var balance = await _transactionRepository.GetBalanceByMonthYearAsync(transaction.TransactionDate);

        decimal usedBudget = balance.Where(j => j.Category.Type == CategoryTypeEnum.Expense).Sum(j => j.Amount);
        decimal incomingBalance = balance.Where(j => j.Category.Type == CategoryTypeEnum.Income).Sum(j => j.Amount) + originalAmount;

        var budgetAmount = incomingBalance;
        if (budgetAmount == 0)
        {
            Notify("O lançamento não pode ser realizado, pois não existem receitas para cobrir o lançamento.");
            return false;
        }

        // Calculate the used budget percentage
        decimal usedPercentage = ((usedBudget + transaction.Amount) / budgetAmount) * 100;

        if (usedPercentage > 100)
        {
            Notify("O lançamento não pode ser realizado, pois está acima do valor de receitas.");
            return false;
        }

        if (usedPercentage > 80 && usedPercentage < 100)
            Notify("O saldo de despesa sobre as receitas está acima de 80%.", NotificationTypeEnum.Warning);
        else if (usedPercentage == 100)
            Notify("O saldo de despesa sobre as receitas atingiu 100%.", NotificationTypeEnum.Warning);

        return true;
    }
}
