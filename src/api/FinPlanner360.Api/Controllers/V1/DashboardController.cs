using FinPlanner360.Api.ViewModels.Category;
using FinPlanner360.Api.ViewModels.Dashboard;
using FinPlanner360.Business.Interfaces.Repositories;
using FinPlanner360.Business.Interfaces.Services;
using FinPlanner360.Business.Models;
using FinPlanner360.Business.Models.Enums;
using FinPlanner360.Repositories.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinPlanner360.Api.Controllers.V1
{
    [Authorize(Roles = "USER")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class DashboardController : MainController
    {
        private readonly ITransaction_Repository _transactionRepository;

        public DashboardController(ITransaction_Repository transactionRepository,
            IAppIdentityUser appIdentityUser,
            INotificationService notificationService) : base(appIdentityUser, notificationService)
        {
            _transactionRepository = transactionRepository;
        }

        [HttpGet("Cards/{date:datetime?}")]
        [SwaggerOperation(Summary = "Cards de dashboard", Description = "Retorna informações financeiras resumidas do usuário")]
        [ProducesResponseType(typeof(List<CardSumaryViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CardSumaryViewModel>> GetSumaryCardsAsync(DateTime? date)
        {
            date = date.HasValue && date.Value != DateTime.MinValue && date.Value != DateTime.MaxValue 
                ? date.Value 
                : DateTime.Now;
            DateTime startDate = new DateTime(date.Value.Year, date.Value.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddSeconds(-1);

            ICollection<Transaction> transactions = await _transactionRepository.GetTransactionsByRangeAsync(startDate, endDate);

            if (transactions == null) { return GenerateResponse(null, HttpStatusCode.NotFound); }

            CardSumaryViewModel cardSumary = new CardSumaryViewModel
            {
                TotalIncome = transactions.Where(x => x.TransactionDate < date && x.Category.Type == CategoryTypeEnum.Income).Sum(x => x.Amount),
                TotalExpense = transactions.Where(x => x.TransactionDate < date && x.Category.Type == CategoryTypeEnum.Expense).Sum(x => x.Amount),
                TotalBalance = transactions.Where(x => x.TransactionDate < date).Sum(x => x.Category.Type == CategoryTypeEnum.Expense ? (x.Amount * -1.00m) : x.Amount),
                TotalIncomeToday = transactions.Where(x => x.TransactionDate == date && x.Category.Type == CategoryTypeEnum.Income).Sum(x => x.Amount),
                TotalExpenseToday = transactions.Where(x => x.TransactionDate == date && x.Category.Type == CategoryTypeEnum.Expense).Sum(x => x.Amount),
                FutureTotalIncome = transactions.Where(x => x.TransactionDate > date && x.Category.Type == CategoryTypeEnum.Income).Sum(x => x.Amount),
                FutureTotalExpense = transactions.Where(x => x.TransactionDate > date && x.Category.Type == CategoryTypeEnum.Expense).Sum(x => x.Amount)
            };

            return GenerateResponse(cardSumary, HttpStatusCode.OK);
        }

        [HttpGet("Transactions/{date:datetime?}")]
        [SwaggerOperation(Summary = "Resumo de transação por categoria", Description = "Responsável por devolver uma lista das transações por categoria")]
        [ProducesResponseType(typeof(IEnumerable<TransactionDashboardViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<TransactionDashboardViewModel>>> GetCategoryTransactionGraphAsync(DateTime? date)
        {
            date = date.HasValue && date.Value != DateTime.MinValue && date.Value != DateTime.MaxValue
                ? date.Value
                    : DateTime.Now;
            DateTime startDate = new DateTime(date.Value.Year, date.Value.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddSeconds(-1);

            var transactionsList = await _transactionRepository.GetTransactionsWithCategoryByRangeAsync(startDate, endDate);

            var transactionsDashboard = (from x in transactionsList
                                         group x by new { x.Category.Description, x.Category.Type } into g
                                         select new TransactionDashboardViewModel
                                         {
                                             CategoryDescription = g.Key.Description,
                                             Type = g.Key.Type,
                                             TotalAmount = g.Sum(x => x.Amount)
                                         });

            return GenerateResponse(transactionsDashboard, HttpStatusCode.OK);
        }
    }
}
