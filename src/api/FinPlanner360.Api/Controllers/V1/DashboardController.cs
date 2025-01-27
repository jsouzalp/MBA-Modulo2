using FinPlanner360.Api.ViewModels.Dashboard;
using FinPlanner360.Business.Interfaces.Repositories;
using FinPlanner360.Business.Interfaces.Services;
using FinPlanner360.Business.Models;
using FinPlanner360.Business.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace FinPlanner360.Api.Controllers.V1
{
    [Authorize(Roles = "USER")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class DashboardController : MainController
    {
        private readonly ITransactionRepository _transactionRepository;

        public DashboardController(ITransactionRepository transactionRepository,
            IAppIdentityUser appIdentityUser,
            INotificationService notificationService) : base(appIdentityUser, notificationService)
        {
            _transactionRepository = transactionRepository;
        }

        [HttpGet("cards/{date:datetime?}")]
        [SwaggerOperation(Summary = "Cards de dashboard", Description = "Retorna informações financeiras resumidas do usuário")]
        [ProducesResponseType(typeof(List<CardSumaryViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CardSumaryViewModel>> GetSumaryCardsAsync(DateTime? date)
        {
            date = date.HasValue && date.Value != DateTime.MinValue && date.Value != DateTime.MaxValue
                ? date.Value
                : DateTime.Now.Date;
            DateTime startDate = new DateTime(date.Value.Year, date.Value.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddSeconds(-1);
            bool isFuture = startDate > DateTime.Now.Date;
            bool isPast = DateTime.Now.Date > endDate;

            ICollection<Transaction> transactions = await _transactionRepository.GetTransactionsByRangeAsync(startDate, endDate);

            if (transactions == null) { return GenerateResponse(null, HttpStatusCode.NotFound); }

            CardSumaryViewModel cardSumary = new CardSumaryViewModel
            {
                WalletBalance = await _transactionRepository.GetWalletBalanceAsync(startDate),
            };

            if (date.Value.Year == DateTime.Now.Year && date.Value.Month == DateTime.Now.Month)
            {
                cardSumary.TotalIncome = transactions.Where(x => x.TransactionDate < date && x.Category.Type == CategoryTypeEnum.Income).Sum(x => x.Amount);
                cardSumary.TotalExpense = transactions.Where(x => x.TransactionDate < date && x.Category.Type == CategoryTypeEnum.Expense).Sum(x => x.Amount);
                cardSumary.TotalIncomeToday = transactions.Where(x => x.TransactionDate == date && x.Category.Type == CategoryTypeEnum.Income).Sum(x => x.Amount);
                cardSumary.TotalExpenseToday = transactions.Where(x => x.TransactionDate == date && x.Category.Type == CategoryTypeEnum.Expense).Sum(x => x.Amount);
                cardSumary.TotalBalance = transactions.Where(x => x.TransactionDate <= date).Sum(x => x.Category.Type == CategoryTypeEnum.Expense ? (x.Amount * -1.00m) : x.Amount);
                cardSumary.FutureTotalIncome = transactions.Where(x => x.TransactionDate > date && x.Category.Type == CategoryTypeEnum.Income).Sum(x => x.Amount);
                cardSumary.FutureTotalExpense = transactions.Where(x => x.TransactionDate > date && x.Category.Type == CategoryTypeEnum.Expense).Sum(x => x.Amount);
            }
            else if (isFuture)
            {
                cardSumary.FutureTotalIncome = transactions.Where(x => x.Category.Type == CategoryTypeEnum.Income).Sum(x => x.Amount);
                cardSumary.FutureTotalExpense = transactions.Where(x => x.Category.Type == CategoryTypeEnum.Expense).Sum(x => x.Amount);
            }
            else if (isPast)
            {
                cardSumary.TotalIncome = transactions.Where(x => x.Category.Type == CategoryTypeEnum.Income).Sum(x => x.Amount);
                cardSumary.TotalExpense = transactions.Where(x => x.Category.Type == CategoryTypeEnum.Expense).Sum(x => x.Amount);
                cardSumary.TotalBalance = transactions.Sum(x => x.Category.Type == CategoryTypeEnum.Expense ? (x.Amount * -1.00m) : x.Amount);
            }

            //CardSumaryViewModel cardSumary = new CardSumaryViewModel
            //{
            //    WalletBalance = await _transactionRepository.GetWalletBalanceAsync(startDate),
            //    TotalIncome = !isFuture ? transactions.Where(x => x.TransactionDate < date && x.Category.Type == CategoryTypeEnum.Income).Sum(x => x.Amount) : 0.00m,
            //    TotalExpense = !isFuture ? transactions.Where(x => x.TransactionDate < date && x.Category.Type == CategoryTypeEnum.Expense).Sum(x => x.Amount) : 0.00m,
            //    TotalIncomeToday = !isFuture && !isPast ? transactions.Where(x => x.TransactionDate == date && x.Category.Type == CategoryTypeEnum.Income).Sum(x => x.Amount) : 0.00m,
            //    TotalExpenseToday = !isFuture && !isPast ? transactions.Where(x => x.TransactionDate == date && x.Category.Type == CategoryTypeEnum.Expense).Sum(x => x.Amount) : 0.00m,
            //    TotalBalance = !isFuture ? transactions.Where(x => x.TransactionDate <= date).Sum(x => x.Category.Type == CategoryTypeEnum.Expense ? (x.Amount * -1.00m) : x.Amount) : 0.00m,
            //    FutureTotalIncome = isFuture ? transactions.Where(x => x.TransactionDate > date && x.Category.Type == CategoryTypeEnum.Income).Sum(x => x.Amount) : 0.00m,
            //    FutureTotalExpense = isFuture ? transactions.Where(x => x.TransactionDate > date && x.Category.Type == CategoryTypeEnum.Expense).Sum(x => x.Amount) : 0.00m
            //};

            return GenerateResponse(cardSumary, HttpStatusCode.OK);
        }

        [HttpGet("transactions/{date:datetime?}")]
        [SwaggerOperation(Summary = "Resumo de transação por categoria", Description = "Responsável por devolver uma lista das transações por categoria")]
        [ProducesResponseType(typeof(IEnumerable<TransactionDashboardViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<TransactionDashboardViewModel>>> GetCategoryTransactionGraphAsync(DateTime? date)
        {
            date = date.HasValue && date.Value != DateTime.MinValue && date.Value != DateTime.MaxValue
                ? date.Value
                : DateTime.Now.Date;
            DateTime startDate = new DateTime(date.Value.Year, date.Value.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddSeconds(-1);

            var transactionsList = await _transactionRepository.GetTransactionsWithCategoryByRangeAsync(startDate, endDate);

            var transactionsDashboard = (from x in transactionsList
                                         group x by new { x.Category.Description, x.Category.Type } into g
                                         select new TransactionDashboardViewModel
                                         {
                                             CategoryDescription = g.Key.Description,
                                             TotalAmount = g.Sum(x => x.Amount),
                                             Quantity = g.Count()
                                         });

            return GenerateResponse(transactionsDashboard, HttpStatusCode.OK);
        }

        [HttpGet("evolution/{date:datetime?}")]
        [SwaggerOperation(Summary = "Evolução do Saldo disponível", Description = "Responsável uma projeção de saldo disponível nos últimos 12 meses")]
        [ProducesResponseType(typeof(IEnumerable<TransactionDashboardViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<TransactionDashboardViewModel>>> GetBalanceAsync(DateTime? date)
        {
            date = date.HasValue && date.Value != DateTime.MinValue && date.Value != DateTime.MaxValue
                ? date.Value
                : DateTime.Now.Date;
            DateTime startDate = new DateTime(date.Value.Year, date.Value.Month, 1).AddMonths(1).AddYears(-1);
            DateTime endDate = startDate.AddYears(1).AddSeconds(-1);

            var transactionsList = await _transactionRepository.GetTransactionsWithCategoryByRangeAsync(startDate, endDate);

            var transactionsGrouped = (from x in transactionsList
                                       group x by new { x.TransactionDate.Year, x.TransactionDate.Month } into g
                                       select new TransactionYearEvolutionViewModel
                                       {
                                           Year = g.Key.Year,
                                           Month = g.Key.Month,
                                           TotalIncome = g.Sum(x => x.Category.Type == CategoryTypeEnum.Income ? x.Amount : 0.00m),
                                           TotalExpense = g.Sum(x => x.Category.Type == CategoryTypeEnum.Expense ? x.Amount : 0.00m),
                                           TotalBalance = g.Sum(x => x.Category.Type == CategoryTypeEnum.Expense ? (x.Amount * -1.00m) : x.Amount)
                                       }).OrderBy(x => x.Year)
                                           .ThenBy(x => x.Month);

            var allMonths = Enumerable.Range(0, 12)
                .Select(x => startDate.AddMonths(x))
                .Select(date => new { date.Year, date.Month })
                .ToList();

            var transactionsDashboard = (from month in allMonths
                                         join transaction in transactionsGrouped
                                         on new { month.Year, month.Month } equals new { transaction.Year, transaction.Month }
                                         into transactionGroup
                                         from tg in transactionGroup.DefaultIfEmpty()
                                         select new TransactionYearEvolutionViewModel
                                         {
                                             Year = month.Year,
                                             Month = month.Month,
                                             TotalIncome = tg?.TotalIncome ?? 0.00m,
                                             TotalExpense = tg?.TotalExpense ?? 0.00m,
                                             TotalBalance = tg?.TotalBalance ?? 0.00m
                                         }).OrderBy(x => x.Year)
                                           .ThenBy(x => x.Month)
                                           .ToList();

            return GenerateResponse(transactionsDashboard, HttpStatusCode.OK);
        }
    }
}
