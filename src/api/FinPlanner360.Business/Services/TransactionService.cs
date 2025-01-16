using AutoMapper;
using FinPlanner360.Busines.Interfaces.Validations;
using FinPlanner360.Busines.Services;
using FinPlanner360.Business.DTO.Transacton;
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
    private readonly IGeneralBudgetRepository _generalBudgetRepository;
    private readonly IValidationFactory<Transaction> _validationFactory;

    public TransactionService(IMapper mapper,
        IValidationFactory<Transaction> validationFactory,
        INotificationService notificationService,
        ITransactionRepository transactionRepository,
        IBudgetRepository budgetRepository,
        IGeneralBudgetRepository generalbudgetRepository) : base(notificationService)
    {
        _mapper = mapper;
        _validationFactory = validationFactory;
        _transactionRepository = transactionRepository;
        _budgetRepository = budgetRepository;
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

        if (await BudgetOkAsync(transaction))
            await _transactionRepository.CreateAsync(transaction.FillAttributes());
    }

    public async Task UpdateAsync(Transaction transactionUpdate)
    {
        var budget = await _transactionRepository.GetByIdAsync(transactionUpdate.TransactionId);
        budget.Amount = transactionUpdate.Amount;
        budget.CategoryId = transactionUpdate.CategoryId;

        if (!await _validationFactory.ValidateAsync(budget))
            return;

        await _transactionRepository.UpdateAsync(budget);
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

    private async Task<bool> BudgetOkAsync(Transaction transaction)
    {
        var generalBudget = (await _generalBudgetRepository.GetAllAsync()).FirstOrDefault();

        if (generalBudget != null && (generalBudget.Percentage.HasValue || generalBudget.Amount.HasValue))
        {
            return await GeneralBudgetOkAsync(transaction, generalBudget);
        }

        return await CategoryBudgetOkAsync(transaction);
    }

    private async Task<bool> GeneralBudgetOkAsync(Transaction transaction, GeneralBudget generalBudget)
    {
        var balance = await _transactionRepository.GetBalanceByMonthYearAsync(transaction.TransactionDate);
        if (balance.Count == 0) return true;

        decimal usedBudget = balance.Where(j => j.Category.Type == CategoryTypeEnum.Expense).Sum(j => j.Amount);
        decimal incomingBalance = balance.Where(j => j.Category.Type == CategoryTypeEnum.Income).Sum(j => j.Amount);

        // Determine the budget amount
        decimal budgetAmount = generalBudget.Amount.GetValueOrDefault() > 0
            ? generalBudget.Amount.Value
            : (incomingBalance * (generalBudget.Percentage.GetValueOrDefault() / 100m));

        // Calculate the used budget percentage
        decimal usedPercentage = ((usedBudget + transaction.Amount) / budgetAmount) * 100;

        if (usedPercentage > 80 && usedPercentage < 100)
        {
            Notify("O saldo está acima de 80%.", NotificationTypeEnum.Warning);

            return true;
        }
        else if (usedPercentage > 100)
        {
            Notify("O lançamento não pode ser realizado, pois está acima do limite estabelecido.");
            return false;
        }

        Notify("O saldo atingiu 100%.", NotificationTypeEnum.Warning);
        return true;
    }

    private async Task<bool> CategoryBudgetOkAsync(Transaction transaction)
    {
        var categoryBudget = await _budgetRepository.GetBudgetByCategoryId(transaction.CategoryId);
        var balance = await _transactionRepository.GetBalanceByMonthYearAsync(transaction.TransactionDate);
        if (balance.Count == 0) return true;

        decimal usedBudget = balance.Where(j => j.Category.Type == CategoryTypeEnum.Expense && j.CategoryId == transaction.CategoryId).Sum(j => j.Amount);
        decimal incomingBalance = balance.Where(j => j.Category.Type == CategoryTypeEnum.Income).Sum(j => j.Amount);

        // Calculate the used budget percentage
        decimal usedPercentage = ((usedBudget + transaction.Amount) / categoryBudget.Amount) * 100;

        if (usedPercentage > 80 && usedPercentage < 100)
        {
            Notify("O saldo dessa categoria está acima de 80%.", NotificationTypeEnum.Warning);

            return true;
        }
        else if (usedPercentage > 100)
        {
            Notify("O lançamento não pode ser realizado, pois está acima do limite estabelecido.");
            return false;
        }

        Notify("O saldo dessa categoria atingiu 100%.", NotificationTypeEnum.Warning);
        return true;
    }
}