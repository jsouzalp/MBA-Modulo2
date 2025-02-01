using AutoMapper;
using FinPlanner360.Api.Extensions;
using FinPlanner360.Api.Reports;
using FinPlanner360.Api.Reports.Closed_Xml;
using FinPlanner360.Api.Reports.Fast;
using FinPlanner360.Api.ViewModels.Report;
using FinPlanner360.Business.Extensions;
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

    [HttpGet("transactions/summary-by-category")]
    [SwaggerOperation(Summary = "Sintético de transação por categoria", Description = "Responsável por devolver uma lista das transações por categoria em um intervalo de datas")]
    [ProducesResponseType(typeof(IEnumerable<TransactionCategoyViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<TransactionCategoyViewModel>>> GetCategoryTransactionSummaryAsync([FromQuery][Required] DateTime startDate, [FromQuery][Required] DateTime endDate)
    {
        if (!IsValidDateRange(startDate, endDate)) { return GenerateResponse(); }

        var transactionsList = await _transactionRepository.GetTransactionsWithCategoryByRangeAsync(startDate.GetStartDate(), endDate.GetEndDate());

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

    [HttpGet("transactions/summary-by-category/export-report")]
    [SwaggerOperation(Summary = "Exporta um relatório de Transacoes por Categorias entre o peridodo Sintético ", Description = "Gera e exporta um relatório contendo informações dos usuários em formato PDF ou XLSX. O tipo de arquivo deve ser especificado no parâmetro `fileType`.")]
    [ProducesResponseType(typeof(TransactionAnalyticsViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ExportReportCategoryTransactionSummaryAsync([FromQuery][Required] DateTime startDate, [FromQuery][Required] DateTime endDate, [FromQuery][Required(ErrorMessage = "O arquivo deve ser informado como tipo PDF ou XLSX")][RegularExpression(@"^(pdf|Pdf|PDF|xlsx|Xlsx|XLSX)$", ErrorMessage = "O arquivo deve ser do tipo PDF ou XLSX.")] string fileType)
    {
        if (!IsValidDateRange(startDate, endDate)) { return GenerateResponse(); }
        if (!ValidateFileType(fileType)) { return GenerateResponse(); }

        var transactionsList = await _transactionRepository.GetTransactionsWithCategoryByRangeAsync(startDate.GetStartDate(), endDate.GetEndDate());
               
        if (!ExistsTransactions(transactionsList)) { return GenerateResponse(); }

        List<TransactionCategoyViewModel> transactionsReport = (from x in transactionsList
                                                                group x by new { x.Category.Description, x.Category.Type } into g
                                                                select new TransactionCategoyViewModel
                                                                {
                                                                    CategoryDescription = g.Key.Description,
                                                                    Type = g.Key.Type.GetDescription(),
                                                                    TotalAmount = g.Sum(x => x.Amount).ToString("C")
                                                                }).ToList();

        var result = GenerateReportToFile.Generate<TransactionCategoyViewModel>(fileType, "Category", transactionsReport);
        return File(result.FileBytes, result.ContentType, result.FileName);
    }

    [HttpGet("transactions/analytics-by-category")]
    [SwaggerOperation(Summary = "Analítico de transação por categoria", Description = "Responsável por devolver uma lista das transações analíticas por categoria em um intervalo de datas")]
    [ProducesResponseType(typeof(TransactionAnalyticsViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TransactionAnalyticsViewModel>> GetCategoryTransactionAnalyticsAsync([FromQuery][Required] DateTime startDate, [FromQuery][Required] DateTime endDate)
    {
        if (!IsValidDateRange(startDate, endDate)) { return GenerateResponse(); }

        var transactionsList = await _transactionRepository.GetTransactionsWithCategoryByRangeAsync(startDate.GetStartDate(), endDate.GetEndDate());

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


    [HttpGet("transactions/analytics-by-category/export-report")]
    [SwaggerOperation(Summary = "Exporta um relatório de Transacoes por Categorias entre o peridodo Analítico", Description = "Gera e exporta um relatório contendo informações dos usuários em formato PDF ou XLSX.O tipo de arquivo deve ser especificado no parâmetro `fileType`.")]
    [ProducesResponseType(typeof(TransactionAnalyticsViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TransactionAnalyticsViewModel>> ExportReportCategoryTransactionAnalyticsAsync([FromQuery][Required] DateTime startDate, [FromQuery][Required] DateTime endDate, [FromQuery][Required(ErrorMessage = "O arquivo deve ser informado como tipo PDF ou XLSX")][RegularExpression(@"^(pdf|Pdf|PDF|xlsx|Xlsx|XLSX)$", ErrorMessage = "O arquivo deve ser do tipo PDF ou XLSX.")] string fileType)
    {
        if (!IsValidDateRange(startDate, endDate)) { return GenerateResponse(); }
        if (!ValidateFileType(fileType)) { return GenerateResponse(); }

        ICollection<Business.Models.Transaction> transactionsList = await _transactionRepository.GetTransactionsWithCategoryByRangeAsync(startDate.GetStartDate(), endDate.GetEndDate());

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

        var result = GenerateReportToFile.Generate<TransactionAnalyticsViewModel>(fileType, "CategoryAnalytics", transactionsReport);
        return File(result.FileBytes, result.ContentType, result.FileName);
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


        /// <summary>
        /// Listar transação por tipo
        /// </summary>
        /// <remarks>Esta rota lista todas as transações por tipo e o período inicial e final.</remarks>
        /// <param name="dataInicio">Data inicio de emissão/atualização da transação. Formato: yyyy-MM-dd Ex: 2025-01-10 </param>
        /// <param name="dataFim">Data fim de emissão/atualização da transação. Formato: yyyy-MM-dd Ex: 2025-01-13</param>
        /// <response code="200">Sucesso na operação!</response>
        /// <response code="204">Sucesso na operação, porém sem conteúdo de resposta.</response>
        /// <response code="400">Dados inconsistentes na requisição ao listar as transações.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="500">Erro interno de servidor.</response>
        [HttpGet("Transactions/ByType")]
        [SwaggerOperation(Tags = new[] { "Transações" })]
        [ProducesResponseType(typeof(IEnumerable<TransactionReportViewModel>), 200)]
        public async Task<ActionResult<IEnumerable<TransactionReportViewModel>>> GetTransactionByType(
           [Required(ErrorMessage = "O campo data inicial é obrigatório")] DateTime dataInicio,
           [Required(ErrorMessage = "O campo Data final é obrigatório")] DateTime dataFim)
        {


            if (dataInicio > dataFim)
            {
                Notify("A data de início não pode ser posterior à data de término.");
                return GenerateResponse();
            }

            var transactionsReportDto = await _transactionReportService.GetTransactionReportByTypeAsync(dataInicio.GetStartDate(), dataFim.GetEndDate());

            if (transactionsReportDto == null || !transactionsReportDto.Any())
            {
                Notify("Nenhuma transação encontrada no intervalo de datas especificado.");
                return GenerateResponse();
            }

            var transactionsReportViewModel = _mapper.Map<IEnumerable<TransactionReportViewModel>>(transactionsReportDto);

            return GenerateResponse(transactionsReportViewModel, HttpStatusCode.OK);
        }

        /// <summary>
        /// Exporta um relatório de Transacoes por tipo entre o período inicial e final.
        /// </summary>
        /// <remarks>Gera e exporta um relatório contendo informações das transações por tipo em formato PDF ou XLSX. O tipo de arquivo deve ser especificado no parâmetro `tipoArquivo`.</remarks>
        /// <param name="dataInicio">Data inicio de emissão/atualização da transação. Formato: yyyy-MM-dd Ex: 2025-01-10 </param>
        /// <param name="dataFim">Data fim de emissão/atualização da transação. Formato: yyyy-MM-dd Ex: 2025-01-13</param>
        /// <param name="tipoArquivo">O tipo de arquivo deve ser especificado como PDF ou XLSX</param>
        /// <response code="200">Sucesso na operação!</response>
        /// <response code="204">Sucesso na operação, porém sem conteúdo de resposta.</response>
        /// <response code="400">Dados inconsistentes na requisição ao listar as transações.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="500">Erro interno de servidor.</response>
        [HttpGet("Transactions/ByType/export-report")]
        [SwaggerOperation(Tags = new[] { "Transações" })]
        [ProducesResponseType(typeof(TransactionReportViewModel), 200)]
        public async Task<IActionResult> ExportReportTransactionByType(
           [Required(ErrorMessage = "O campo data inicial é obrigatório")] DateTime dataInicio,
           [Required(ErrorMessage = "O campo Data final é obrigatório")] DateTime dataFim,
           [Required(ErrorMessage = "O arquivo deve ser informado como tipo PDF ou XLSX")]
           [RegularExpression(@"^(pdf|Pdf|PDF|xlsx|Xlsx|XLSX)$", ErrorMessage = "O arquivo deve ser do tipo PDF ou XLSX.")] string tipoArquivo)
        {

            if (dataInicio > dataFim)
            {
                Notify("A data de início não pode ser posterior à data de término.");
                return GenerateResponse();
            }

            var transactionsReportDto = await _transactionReportService.GetTransactionReportByTypeAsync(dataInicio.GetStartDate(), dataFim.GetEndDate());

            if (transactionsReportDto == null || !transactionsReportDto.Any())
            {
                Notify("Nenhuma transação encontrada no intervalo de datas especificado.");
                return GenerateResponse();
            }

            var transactionsReportViewModel = _mapper.Map<IEnumerable<TransactionReportViewModel>>(transactionsReportDto);



            byte[] fileBytes;
            string contentType;
            string fileName;

            switch (tipoArquivo.ToLower())
            {
                case "pdf":
                    fileBytes = Reports.Fast.ReportService.GenerateReportPDF("TransactionByType", transactionsReportViewModel);
                    contentType = "application/pdf";
                    fileName = "TransacoesPorTipo.pdf";
                    break;

                case "xlsx":
                    fileBytes = Reports.Closed_Xml.ReportService.GenerateXlsxBytes("TransactionByType", transactionsReportViewModel);
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    fileName = "TransacoesPorTipo.xlsx";
                    break;

                default:
                    Notify("Tipo de arquivo inválido. Use 'pdf' ou 'xlsx'.");
                    return GenerateResponse();
            }

            return File(fileBytes, contentType, fileName);
        }


    }
}
