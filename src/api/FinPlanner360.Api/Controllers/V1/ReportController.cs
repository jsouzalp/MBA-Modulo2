using FinPlanner360.Api.Extensions;
using FinPlanner360.Api.ViewModels.Dashboard;
using FinPlanner360.Api.ViewModels.Report;
using FinPlanner360.Business.Interfaces.Repositories;
using FinPlanner360.Business.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection;

namespace FinPlanner360.Api.Controllers.V1
{
    [Authorize(Roles = "USER")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class ReportController : MainController
    {
        private readonly ITransaction_Repository _transactionRepository;
        public ReportController(ITransaction_Repository transactionRepository,
                                IAppIdentityUser appIdentityUser, 
                                INotificationService notificationService) : base(appIdentityUser, notificationService)
        {

            _transactionRepository = transactionRepository;

        }

        [HttpGet("Transactions/SummaryByCategory")]
        [SwaggerOperation(Summary = "Sintético de transação por categoria", Description = "Responsável por devolver uma lista das transações por categoria em um intervalo de datas")]
        [ProducesResponseType(typeof(IEnumerable<TransactionCategoyViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<TransactionCategoyViewModel>>> GetCategoryTransactionSummaryAsync([FromQuery][Required] DateTime startDate,[FromQuery][Required] DateTime endDate)
        {
            if (startDate > endDate)
                return BadRequest("A data de início não pode ser posterior à data de término.");

            var transactionsList = await _transactionRepository.GetTransactionsWithCategoryByRangeAsync(startDate, endDate);

            if (transactionsList == null && !transactionsList.Any())
                return NotFound("Nenhuma transação encontrada no intervalo de datas especificado.");

            var transactionsReport = (from x in transactionsList
                                      group x by new { x.Category.Description, x.Type } into g
                                      select new TransactionCategoyViewModel
                                      {
                                          CategoryDescription = g.Key.Description,
                                          Type = g.Key.Type,
                                          TotalAmount = g.Sum(x => x.Amount)
                                      });

            return GenerateResponse(transactionsReport, HttpStatusCode.OK);
        }


        [HttpGet("Transactions/AnalyticsByCategory")]
        [SwaggerOperation(Summary = "Analítico de transação por categoria", Description = "Responsável por devolver uma lista das transações analíticas por categoria em um intervalo de datas")]
        [ProducesResponseType(typeof(TransactionAnalyticsViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<TransactionAnalyticsViewModel>> GetCategoryTransactionAnalyticsAsync([FromQuery][Required] DateTime startDate, [FromQuery][Required] DateTime endDate)
        {
            if (startDate > endDate)
                return BadRequest("A data de início não pode ser posterior à data de término.");

            var transactionsList = await _transactionRepository.GetTransactionsWithCategoryByRangeAsync(startDate, endDate);

            if (transactionsList == null || !transactionsList.Any())
                return NotFound("Nenhuma transação encontrada no intervalo de datas especificado.");


            var groupedTransactionsReport = transactionsList
                    .Select(x => new TransactionAnalyticsViewModel
                    {
                        TransactionDate = x.TransactionDate.ToString("dd/MM/yyyy"),
                        Type = x.Type.GetDescription(),
                        Description = x.Description,
                        CategoryDescription = x.Category.Description,
                        TotalAmount = x.Amount.ToString("C"),
                    })
                    .GroupBy(x => x.CategoryDescription)
                    .Select(group => new GroupedTransactionAnalyticsViewModel
                    {
                        CategoryDescription = group.Key,
                        Transactions = group.OrderBy(x => x.TransactionDate).ToList()
                    })
                    .ToList();


            return GenerateResponse(groupedTransactionsReport, HttpStatusCode.OK);
        }


    }

    
}
