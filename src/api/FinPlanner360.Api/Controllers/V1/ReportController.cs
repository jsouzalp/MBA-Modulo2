using FastReport.Web;
using FinPlanner360.Api.Extensions;
using FinPlanner360.Api.ViewModels.Report;
using FinPlanner360.Business.Interfaces.Repositories;
using FinPlanner360.Business.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace FinPlanner360.Api.Controllers.V1;

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
    public async Task<ActionResult<IEnumerable<TransactionCategoyViewModel>>> GetCategoryTransactionSummaryAsync([FromQuery][Required] DateTime startDate, [FromQuery][Required] DateTime endDate)
    {
        if (!IsValidDateRange(startDate, endDate)) { return GenerateResponse(); }

        var transactionsList = await _transactionRepository.GetTransactionsWithCategoryByRangeAsync(startDate, endDate);

        if (!ExistsTransactions(transactionsList)) { return GenerateResponse(); }

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

    [HttpGet("Transactions/SummaryByCategory/export-report")]
    [SwaggerOperation(Summary = "Exporta um relatório de Transacoes por Categorias entre o peridodo Sintético ", Description = "Gera e exporta um relatório contendo informações dos usuários em formato PDF ou XLSX. O tipo de arquivo deve ser especificado no parâmetro `fileType`.")]
    [ProducesResponseType(typeof(TransactionAnalyticsViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ExportReportCategoryTransactionSummaryAsync([FromQuery][Required] DateTime startDate, [FromQuery][Required] DateTime endDate, [FromQuery][Required(ErrorMessage = "O arquivo deve ser informado como tipo PDF ou XLSX")][RegularExpression(@"^(pdf|Pdf|PDF|xlsx|Xlsx|XLSX)$", ErrorMessage = "O arquivo deve ser do tipo PDF ou XLSX.")] string fileType)
    {
        if (!IsValidDateRange(startDate, endDate)) { return GenerateResponse(); }
        if (!ValidateFileType(fileType)) { return GenerateResponse(); }

        var transactionsList = await _transactionRepository.GetTransactionsWithCategoryByRangeAsync(startDate, endDate);

        if (!ExistsTransactions(transactionsList)) { return GenerateResponse(); }

        List<TransactionCategoyViewModel> transactionsReport = (from x in transactionsList
                                                                group x by new { x.Category.Description, x.Category.Type } into g
                                                                select new TransactionCategoyViewModel
                                                                {
                                                                    CategoryDescription = g.Key.Description,
                                                                    Type = g.Key.Type.GetDescription(),
                                                                    TotalAmount = g.Sum(x => x.Amount).ToString("C")
                                                                }).ToList();

        return GenerateReportToFile<TransactionCategoyViewModel>(fileType, "Category", transactionsReport);
    }

    [HttpGet("Transactions/AnalyticsByCategory")]
    [SwaggerOperation(Summary = "Analítico de transação por categoria", Description = "Responsável por devolver uma lista das transações analíticas por categoria em um intervalo de datas")]
    [ProducesResponseType(typeof(TransactionAnalyticsViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TransactionAnalyticsViewModel>> GetCategoryTransactionAnalyticsAsync([FromQuery][Required] DateTime startDate, [FromQuery][Required] DateTime endDate)
    {
        if (!IsValidDateRange(startDate, endDate)) { return GenerateResponse(); }

        var transactionsList = await _transactionRepository.GetTransactionsWithCategoryByRangeAsync(startDate, endDate);

        if (!ExistsTransactions(transactionsList)) { return GenerateResponse(); }

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


    [HttpGet("Transactions/AnalyticsByCategory/export-report")]
    [SwaggerOperation(Summary = "Exporta um relatório de Transacoes por Categorias entre o peridodo Analítico", Description = "Gera e exporta um relatório contendo informações dos usuários em formato PDF ou XLSX.O tipo de arquivo deve ser especificado no parâmetro `fileType`.")]
    [ProducesResponseType(typeof(TransactionAnalyticsViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TransactionAnalyticsViewModel>> ExportReportCategoryTransactionAnalyticsAsync([FromQuery][Required] DateTime startDate, [FromQuery][Required] DateTime endDate, [FromQuery][Required(ErrorMessage = "O arquivo deve ser informado como tipo PDF ou XLSX")][RegularExpression(@"^(pdf|Pdf|PDF|xlsx|Xlsx|XLSX)$", ErrorMessage = "O arquivo deve ser do tipo PDF ou XLSX.")] string fileType)
    {
        if (!IsValidDateRange(startDate, endDate)) { return GenerateResponse(); }
        if (!ValidateFileType(fileType)) { return GenerateResponse(); }

        ICollection<Business.Models.Transaction> transactionsList = await _transactionRepository.GetTransactionsWithCategoryByRangeAsync(startDate, endDate);

        if (!ExistsTransactions(transactionsList)) { return GenerateResponse(); }

        List<TransactionAnalyticsViewModel> transactionsReport = transactionsList
                                            .Select(x => new TransactionAnalyticsViewModel
                                            {
                                                TransactionDate = x.TransactionDate.ToString("dd/MM/yyyy"),
                                                Type = x.Category.Type.GetDescription(),
                                                Description = x.Description,
                                                CategoryDescription = x.Category.Description,
                                                TotalAmount = x.Amount.ToString("C"),
                                            })
                                            .OrderBy(x => x.TransactionDate)
                                            .ToList();

        return GenerateReportToFile<TransactionAnalyticsViewModel>(fileType, "CategoryAnalytics", transactionsReport);
    }

    private bool IsValidDateRange(DateTime startDate, DateTime endDate)
    {
        if (startDate.Date > endDate.Date)
        {
            Notify("A data de início não pode ser posterior à data de término.");
            return false;
        }
        return true;
    }

    private bool ExistsTransactions(IEnumerable<Business.Models.Transaction> transactionsList)
    {
        if (transactionsList == null || !transactionsList.Any())
        {
            Notify("Nenhuma transação encontrada no intervalo de datas especificado.");
            return false;
        }
        return true;
    }

    private bool ValidateFileType(string fileType)
    {
        if (string.Equals(fileType, "pdf", StringComparison.OrdinalIgnoreCase) || string.Equals(fileType, "xlsx", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        Notify("Tipo de arquivo inválido. Use 'pdf' ou 'xlsx'.");
        return false;
    }

    private FileContentResult GenerateReportToFile<T>(string fileType, string reportName, IEnumerable<T> transactionsReport)
    {
        byte[] fileBytes = null;
        string contentType = null;
        string fileName = null;

        switch (fileType.ToLower())
        {
            case "pdf":
                fileBytes = Reports.Fast.ReportService.GenerateReportPDF(reportName, transactionsReport);
                contentType = "application/pdf";
                fileName = "Categorias.pdf";
                break;

            case "xlsx":
                fileBytes = Reports.Closed_Xml.ReportService.GenerateXlsxBytes(reportName, transactionsReport);
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "Categorias.xlsx";
                break;
        }

        return File(fileBytes, contentType, fileName);
    }
}
