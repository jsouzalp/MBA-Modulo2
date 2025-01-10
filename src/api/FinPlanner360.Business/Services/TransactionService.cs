using FinPlanner360.Busines.Interfaces.Validations;
using FinPlanner360.Busines.Services;
using FinPlanner360.Business.Extensions;
using FinPlanner360.Business.Interfaces.Repositories;
using FinPlanner360.Business.Interfaces.Services;
using FinPlanner360.Business.Models.Enums;
using Transaction = FinPlanner360.Business.Models.Transaction;

namespace FinPlanner360.Business.Services;

public class TransactionService : BaseService, ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IBudgetRepository _budgetRepository;
    private readonly IValidationFactory<Transaction> _validationFactory;

    public TransactionService(IValidationFactory<Transaction> validationFactory,
        INotificationService notificationService,
        ITransactionRepository transactionRepository,
        IBudgetRepository budgetRepository) : base(notificationService)
    {
        _validationFactory = validationFactory;
        _transactionRepository = transactionRepository;
    }

    public async Task<ICollection<TransactionViewModel>> GetAllAsync()
    {
        var transactions = await _transactionRepository.GetAllAsync();
        var categoryBalance = await _budgetRepository.GetAllAsync();

        foreach (var transaction in transactions)
        {
            if (transaction.Type == TransactionTypeEnum.Expense)
                transaction.Amount = -transaction.Amount;

            transaction.
        }
        return transactions;
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