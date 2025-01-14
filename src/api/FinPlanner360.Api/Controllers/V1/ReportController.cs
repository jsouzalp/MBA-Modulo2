using FinPlanner360.Api.ViewModels.Dashboard;
using FinPlanner360.Api.ViewModels.Report;
using FinPlanner360.Business.Interfaces.Repositories;
using FinPlanner360.Business.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

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
        [SwaggerOperation(Summary = "Sintético de transação por categoria", Description = "Responsável por devolver uma lista das transações por categoria")]
        [ProducesResponseType(typeof(IEnumerable<TransactionCategoyViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<TransactionCategoyViewModel>>> GetCategoryTransactionSummaryAsync([FromQuery] DateTime startDate,[FromQuery] DateTime endDate)
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
        [SwaggerOperation(Summary = "Análise detalhada das transações", Description = "Retorna métricas analíticas detalhadas das transações em um intervalo de datas")]
        [ProducesResponseType(typeof(TransactionAnalyticsViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<TransactionAnalyticsViewModel>> GetCategoryTransactionAnalyticsAsync([FromQuery] DateTime startDate,[FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
                return BadRequest("A data de início não pode ser posterior à data de término.");

            var transactionsList = await _transactionRepository.GetTransactionsWithCategoryByRangeAsync(startDate, endDate);

            if (transactionsList == null && !transactionsList.Any())
                return NotFound("Nenhuma transação encontrada no intervalo de datas especificado.");


            var transactionsReport = transactionsList
                                     .Select(x => new TransactionAnalyticsViewModel
                                     {
                                         TransactionDate = x.TransactionDate,
                                         CategoryDescription = x.Category.Description,
                                         Type = x.Type,
                                         TotalAmount = x.Amount,
                                     })
                                     .OrderBy(x => x.TransactionDate); 


            return GenerateResponse(transactionsReport, HttpStatusCode.OK);


        }


    }
}
