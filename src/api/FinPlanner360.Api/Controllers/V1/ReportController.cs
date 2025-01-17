using FinPlanner360.Api.Extensions;
using FinPlanner360.Api.ViewModels.Dashboard;
using FinPlanner360.Api.ViewModels.Report;
using FinPlanner360.Business.Interfaces.Repositories;
using FinPlanner360.Business.Interfaces.Services;
using FinPlanner360.Repositories.Repositories;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ITransactionRepository _transactionRepository;
        public ReportController(ITransactionRepository transactionRepository,
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
                                      group x by new { x.Category.Description, x.Category.Type } into g
                                      select new TransactionCategoyViewModel
                                      {
                                          CategoryDescription = g.Key.Description,
                                          Type = g.Key.Type.GetDescription(),
                                          TotalAmount = g.Sum(x => x.Amount).ToString("C")
                                      });

            return GenerateResponse(transactionsReport, HttpStatusCode.OK);
        }



        [AllowAnonymous]
        [HttpGet("Transactions/SummaryByCategory/export-report")]
        [SwaggerOperation(Summary = "Exporta um relatório de Transacoes por Categorias entre o peridodo ", Description = "Gera e exporta um relatório contendo informações dos usuários em formato PDF ou XLSX. O tipo de arquivo deve ser especificado no parâmetro `fileType`.")]
        [ProducesResponseType(typeof(TransactionAnalyticsViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ExportReportCategoryTransactionSummaryAsync([FromQuery][Required] DateTime startDate, [FromQuery][Required] DateTime endDate, [FromQuery][Required(ErrorMessage = "O arquivo deve ser informado como tipo PDF ou XLSX")][RegularExpression(@"^(pdf|Pdf|PDF|xlsx|Xlsx|XLSX)$", ErrorMessage = "O arquivo deve ser do tipo PDF ou XLSX.")] string fileType)
        {
            if (startDate > endDate)
                return BadRequest("A data de início não pode ser posterior à data de término.");

            var transactionsList = await _transactionRepository.GetTransactionsWithCategoryByRangeAsync(startDate, endDate);

            if (transactionsList == null && !transactionsList.Any())
                return NotFound("Nenhuma transação encontrada no intervalo de datas especificado.");

            List<TransactionCategoyViewModel> transactionsReport = (from x in transactionsList
                                    group x by new { x.Category.Description, x.Category.Type } into g
                                    select new TransactionCategoyViewModel
                                    {
                                        CategoryDescription = g.Key.Description,
                                        Type = g.Key.Type.GetDescription(),
                                        TotalAmount = g.Sum(x => x.Amount).ToString("C")
                                    }).ToList();

            byte[] fileBytes;
            string contentType;
            string fileName;

            switch (fileType.ToLower())
            {
                case "pdf":
                    fileBytes = Reports.Fast.ReportService.GenerateReportPDF("Category", transactionsReport);
                    contentType = "application/pdf";
                    fileName = "Categorias.pdf";
                    break;

                case "xlsx":
                    fileBytes = Reports.Closed_Xml.ReportService.GenerateXlsxBytes("Category", transactionsReport);
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    fileName = "Categorias.xlsx";
                    break;

                default:
                    return BadRequest("Tipo de arquivo inválido. Use 'pdf' ou 'xlsx'.");
            }

            return File(fileBytes, contentType, fileName);
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
                        Type = x.Category.Type.GetDescription(),
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
