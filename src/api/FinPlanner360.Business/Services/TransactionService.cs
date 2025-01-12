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
    private readonly IGeneralBudgetRepository _generalbudgetRepository;
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
        _generalbudgetRepository = generalbudgetRepository;
    }

    public async Task<ICollection<BalanceDTO>> GetBalanceByMonthAsync(DateTime date)
    {
        ICollection<Budget> categoryBudget = [];
        var generalBudget = (await _generalbudgetRepository.GetAllAsync()).FirstOrDefault();

        if (generalBudget == null)
            categoryBudget = await _budgetRepository.GetAllAsync();

        var balances = _mapper.Map<ICollection<BalanceDTO>>(await _transactionRepository.GetBalanceByMonthAsync(date));

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
            if (balance.Type == TransactionTypeEnum.Expense)
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
}