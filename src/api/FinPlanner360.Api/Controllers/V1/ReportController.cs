using AutoMapper;
using FinPlanner360.Api.Extensions;
using FinPlanner360.Api.Reports;
using FinPlanner360.Api.ViewModels.Report;
using FinPlanner360.Business.Extensions;
using FinPlanner360.Business.Interfaces.Repositories;
using FinPlanner360.Business.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace FinPlanner360.Api.Controllers.V1;

[Authorize(Roles = "USER")]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[Controller]")]
public class ReportController : MainController
{
    private readonly IMapper _mapper;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITransactionReportService _transactionReportService;
    public ReportController(IMapper mapper,
                            ITransactionRepository transactionRepository,
                            ITransactionReportService transactionReportService,
                            IAppIdentityUser appIdentityUser,
                            INotificationService notificationService) : base(appIdentityUser, notificationService)
    {
        _mapper = mapper;
        _transactionRepository = transactionRepository;
        _transactionReportService = transactionReportService;

    }

    /// <summary>
    /// Sintético de transação por categoria
    /// </summary>
    /// <remarks>Rota responsável por devolver uma lista das transações por categoria em um intervalo de datas.</remarks>
    /// <param name="startDate">Data inicio de emissão/atualização da transação. Formato: yyyy-MM-dd Ex: 2025-01-10 </param>
    /// <param name="endDate">Data fim de emissão/atualização da transação. Formato: yyyy-MM-dd Ex: 2025-01-13</param>
    /// <response code="200">Sucesso na operação!</response>
    /// <response code="404">Página não encontrada.</response>
    /// <response code="401">Usuário não autenticado.</response>
    /// <response code="500">Erro interno de servidor.</response>
    [HttpGet("Transactions/summary-by-category")]
    [ProducesResponseType(typeof(IEnumerable<TransactionCategoyViewModel>), 200)]
    public async Task<ActionResult<IEnumerable<TransactionCategoyViewModel>>> GetCategoryTransactionSummaryAsync(
        [FromQuery][Required] DateTime startDate, 
        [FromQuery][Required] DateTime endDate)
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

    /// <summary>
    /// Exporta um relatório de Transacoes por Categorias entre o peridodo Sintético
    /// </summary>
    /// <remarks>Gera e exporta um relatório contendo informações dos usuários em formato PDF ou XLSX.O tipo de arquivo deve ser especificado no parâmetro `fileType`.</remarks>
    /// <param name="startDate">Data inicio de emissão da transação. Formato: yyyy-MM-dd Ex: 2025-01-10 </param>
    /// <param name="endDate">Data fim de emissão da transação. Formato: yyyy-MM-dd Ex: 2025-01-13</param>
    /// <param name="fileType">O tipo do arquivo deve ser PDF ou XLSX</param>
    /// <response code="200">Sucesso na operação!</response>
    /// <response code="400">Dados inconsistentes na requisição ao listar as transações.</response>
    /// <response code="401">Usuário não autenticado.</response>
    /// <response code="500">Erro interno de servidor.</response> 
    [HttpGet("Transactions/summary-by-category/export-report")]
    [ProducesResponseType(typeof(TransactionAnalyticsViewModel), 200)]
    public async Task<IActionResult> ExportReportCategoryTransactionSummaryAsync(
        [FromQuery][Required] DateTime startDate, 
        [FromQuery][Required] DateTime endDate, 
        [FromQuery][Required(ErrorMessage = "O arquivo deve ser informado como tipo PDF ou XLSX")]
        [RegularExpression(@"^(pdf|Pdf|PDF|xlsx|Xlsx|XLSX)$", ErrorMessage = "O arquivo deve ser do tipo PDF ou XLSX.")] string fileType)
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

    /// <summary>
    /// Analítico de transação por categoria
    /// </summary>
    /// <remarks>Responsável por devolver uma lista das transações analíticas por categoria em um intervalo de datas</remarks>
    /// <param name="startDate">Data inicio de emissão/atualização da transação. Formato: yyyy-MM-dd Ex: 2025-01-10 </param>
    /// <param name="endDate">Data fim de emissão/atualização da transação. Formato: yyyy-MM-dd Ex: 2025-01-13</param>
    /// <response code="200">Sucesso na operação!</response>
    /// <response code="400">Dados inconsistentes na requisição ao listar as transações.</response>
    /// <response code="401">Usuário não autenticado.</response>
    /// <response code="500">Erro interno de servidor.</response>
    [HttpGet("Transactions/analytics-by-category")]
    [ProducesResponseType(typeof(TransactionAnalyticsViewModel), 200)]
    public async Task<ActionResult<TransactionAnalyticsViewModel>> GetCategoryTransactionAnalyticsAsync(
        [FromQuery][Required] DateTime startDate, 
        [FromQuery][Required] DateTime endDate)
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

    /// <summary>
    /// Exporta um relatório de Transacoes por Categorias entre o peridodo Analítico
    /// </summary>
    /// <remarks>Gera e exporta um relatório contendo informações dos usuários em formato PDF ou XLSX.O tipo de arquivo deve ser especificado no parâmetro `fileType`.</remarks>
    /// <param name="startDate">Data inicio de emissão da transação. Formato: yyyy-MM-dd Ex: 2025-01-10 </param>
    /// <param name="endDate">Data fim de emissão da transação. Formato: yyyy-MM-dd Ex: 2025-01-13</param>
    /// <param name="fileType">O tipo do arquivo deve ser PDF ou XLSX</param>
    /// <response code="200">Sucesso na operação!</response>
    /// <response code="400">Dados inconsistentes na requisição ao listar as transações.</response>
    /// <response code="401">Usuário não autenticado.</response>
    /// <response code="500">Erro interno de servidor.</response> 
    [HttpGet("Transactions/analytics-by-category/export-report")]
    [ProducesResponseType(typeof(TransactionAnalyticsViewModel), 200)]
    public async Task<ActionResult<TransactionAnalyticsViewModel>> ExportReportCategoryTransactionAnalyticsAsync(
        [FromQuery][Required] DateTime startDate, 
        [FromQuery][Required] DateTime endDate, 
        [FromQuery][Required(ErrorMessage = "O arquivo deve ser informado como tipo PDF ou XLSX")]
        [RegularExpression(@"^(pdf|Pdf|PDF|xlsx|Xlsx|XLSX)$", ErrorMessage = "O arquivo deve ser do tipo PDF ou XLSX.")] string fileType)
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

    /// <summary>
    /// Listar transação por tipo
    /// </summary>
    /// <remarks>Esta rota lista todas as transações por tipo e o período inicial e final.</remarks>
    /// <param name="startDate">Data inicio de emissão/atualização da transação. Formato: yyyy-MM-dd Ex: 2025-01-10 </param>
    /// <param name="endDate">Data fim de emissão/atualização da transação. Formato: yyyy-MM-dd Ex: 2025-01-13</param>
    /// <response code="200">Sucesso na operação!</response>
    /// <response code="400">Dados inconsistentes na requisição ao listar as transações.</response>
    /// <response code="401">Usuário não autenticado.</response>
    /// <response code="500">Erro interno de servidor.</response>
    [HttpGet("Transactions/by-type")]
    [ProducesResponseType(typeof(IEnumerable<TransactionReportViewModel>), 200)]
    public async Task<ActionResult<IEnumerable<TransactionReportViewModel>>> GetTransactionByType(
        [FromQuery][Required] DateTime startDate,
        [FromQuery][Required] DateTime endDate)
    {

        if (!IsValidDateRange(startDate, endDate)) { return GenerateResponse(); }

        var transactionsReportDto = await _transactionReportService.GetTransactionReportByTypeAsync(startDate.GetStartDate(), endDate.GetEndDate());

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
    /// <remarks>Gera e exporta um relatório contendo informações das transações por tipo em formato PDF ou XLSX. O tipo de arquivo deve ser especificado no parâmetro `fileType`.</remarks>
    /// <param name="startDate">Data inicio de emissão/atualização da transação. Formato: yyyy-MM-dd Ex: 2025-01-10 </param>
    /// <param name="endDate">Data fim de emissão/atualização da transação. Formato: yyyy-MM-dd Ex: 2025-01-13</param>
    /// <param name="fileType">O tipo de arquivo deve ser especificado como PDF ou XLSX</param>
    /// <response code="200">Sucesso na operação!</response>
    /// <response code="400">Dados inconsistentes na requisição ao listar as transações.</response>
    /// <response code="401">Usuário não autenticado.</response>
    /// <response code="500">Erro interno de servidor.</response>
    [HttpGet("Transactions/by-type/export-report")]
    [ProducesResponseType(typeof(TransactionReportViewModel), 200)]
    public async Task<IActionResult> ExportReportTransactionByType(
        [FromQuery][Required] DateTime startDate,
        [FromQuery][Required] DateTime endDate,
        [FromQuery][Required(ErrorMessage = "O arquivo deve ser informado como tipo PDF ou XLSX")]
        [RegularExpression(@"^(pdf|Pdf|PDF|xlsx|Xlsx|XLSX)$", ErrorMessage = "O arquivo deve ser do tipo PDF ou XLSX.")] string fileType)
    {

        if (!IsValidDateRange(startDate, endDate)) { return GenerateResponse(); }
        if (!ValidateFileType(fileType)) { return GenerateResponse(); }

        var transactionsReportDto = await _transactionReportService.GetTransactionReportByTypeAsync(startDate.GetStartDate(), endDate.GetEndDate());

        if (transactionsReportDto == null || !transactionsReportDto.Any())
        {
            Notify("Nenhuma transação encontrada no intervalo de datas especificado.");
            return GenerateResponse();
        }

        var transactionsReportViewModel = transactionsReportDto.Select(t => new TransactionReportViewModel
        {
            Type = t.Type.GetDescription(),
            TotalAmount = t.TotalAmount,
            FormattedTotalAmount = t.TotalAmount.ToString("C", new System.Globalization.CultureInfo("pt-BR")),
            TransactionCount = t.TransactionCount
        }).ToList();

        var result = GenerateReportToFile.Generate<TransactionReportViewModel>(fileType, "TransactionByType", transactionsReportViewModel);
        return File(result.FileBytes, result.ContentType, result.FileName);

    }

    /// <summary>
    /// Listar extrato de transações
    /// </summary>
    /// <remarks>Esta rota lista todas as transações em um período específico.</remarks>
    /// <param name="startDate">Data início de emissão/atualização da transação. Formato: yyyy-MM-dd Ex: 2025-01-10 </param>
    /// <param name="endDate">Data fim de emissão/atualização da transação. Formato: yyyy-MM-dd Ex: 2025-01-13</param>
    /// <response code="200">Sucesso na operação!</response>
    /// <response code="400">Dados inconsistentes na requisição ao listar as transações.</response>
    /// <response code="401">Usuário não autenticado.</response>
    /// <response code="500">Erro interno de servidor.</response>
    [HttpGet("Transactions/Statement")]
    [ProducesResponseType(typeof(IEnumerable<TransactionStatementViewModel>), 200)]
    public async Task<ActionResult<IEnumerable<TransactionStatementViewModel>>> GetTransactionStatement(
        [FromQuery][Required] DateTime startDate,
        [FromQuery][Required] DateTime endDate)
    {

        if (!IsValidDateRange(startDate, endDate)) { return GenerateResponse(); }

        var transactionStatementsDto = await _transactionReportService.GetTransactionStatementAsync(startDate.GetStartDate(), endDate.GetEndDate());

        if (transactionStatementsDto == null || !transactionStatementsDto.Any())
        {
            Notify("Nenhuma transação encontrada no intervalo de datas especificado.");
            return GenerateResponse();
        }

        var transactionStatementsViewModel = _mapper.Map<IEnumerable<TransactionStatementViewModel>>(transactionStatementsDto);

        return GenerateResponse(transactionStatementsViewModel, HttpStatusCode.OK);
    }


    /// <summary>
    /// Exporta um extrato de transações entre o período inicial e final.
    /// </summary>
    /// <remarks>Gera e exporta um extrato contendo informações das transações em formato PDF ou XLSX. O tipo de arquivo deve ser especificado no parâmetro `fileType`.</remarks>
    /// <param name="startDate">Data início de emissão/atualização da transação. Formato: yyyy-MM-dd Ex: 2025-01-10 </param>
    /// <param name="endDate">Data fim de emissão/atualização da transação. Formato: yyyy-MM-dd Ex: 2025-01-13</param>
    /// <param name="fileType">O tipo do arquivo deve ser PDF ou XLSX</param>
    /// <response code="200">Sucesso na operação!</response>
    /// <response code="400">Dados inconsistentes na requisição ao listar as transações.</response>
    /// <response code="401">Usuário não autenticado.</response>
    /// <response code="500">Erro interno de servidor.</response>
    [HttpGet("Transactions/Statement/export-report")]
    [ProducesResponseType(typeof(TransactionStatementViewModel), 200)]
    public async Task<IActionResult> ExportTransactionStatement(
        [FromQuery][Required] DateTime startDate,
        [FromQuery][Required] DateTime endDate,
        [FromQuery][Required(ErrorMessage = "O arquivo deve ser informado como tipo PDF ou XLSX")]
        [RegularExpression(@"^(pdf|Pdf|PDF|xlsx|Xlsx|XLSX)$", ErrorMessage = "O arquivo deve ser do tipo PDF ou XLSX.")] string fileType)
    {

        if (!IsValidDateRange(startDate, endDate)) { return GenerateResponse(); }
        if (!ValidateFileType(fileType)) { return GenerateResponse(); }

        var transactionStatementsDto = await _transactionReportService.GetTransactionStatementAsync(startDate.GetStartDate(), endDate.GetEndDate());

        if (transactionStatementsDto == null || !transactionStatementsDto.Any())
        {
            Notify("Nenhuma transação encontrada no intervalo de datas especificado.");
            return GenerateResponse();
        }

        var transactionStatementsViewModel = transactionStatementsDto.Select(t => new TransactionStatementViewModel
        {
            TransactionId = t.TransactionId,
            Description = t.Description,
            Amount = t.Amount,
            FormattedAmount = t.Amount.ToString("C", new System.Globalization.CultureInfo("pt-BR")),
            Category = t.Category,
            TransactionDate = t.TransactionDate.ToString("dd/MM/yyyy"),
        }).ToList();


        var result = GenerateReportToFile.Generate<TransactionStatementViewModel>(fileType, "TransactionStatement", transactionStatementsViewModel);
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
        return true;
    }
    
}
