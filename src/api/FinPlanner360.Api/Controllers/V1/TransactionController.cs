using AutoMapper;
using FinPlanner360.Api.ViewModels.Transaction;
using FinPlanner360.Business.Interfaces.Services;
using FinPlanner360.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace FinPlanner360.Api.Controllers.V1;

[Authorize(Roles = "USER")]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[Controller]")]
public class TransactionController : MainController
{
    private readonly IMapper _mapper;
    private readonly ITransactionService _transactionService;

    public TransactionController(IMapper mapper,
                            ITransactionService transactionService,
                            IAppIdentityUser appIdentityUser,
                            INotificationService notificationService) : base(appIdentityUser, notificationService)
    {
        _mapper = mapper;
        _transactionService = transactionService;
    }
    
    /// <summary>
    /// </summary>
    /// <remarks></remarks>
    /// <response code="200">Sucesso na operação!</response>
    /// <response code="401">Usuário não autenticado.</response>
    /// <response code="404">Página não encontrada.</response>
    /// <response code="500">Erro interno de servidor.</response>
    [HttpGet("get-balance-by-month-year")]
    [ProducesResponseType(typeof(List<BalanceViewModel>), 200)]
    public async Task<ActionResult<IEnumerable<BalanceViewModel>>> GetBalanceByMonthYear([FromQuery] DateTime date)
    {
        var transactions = _mapper.Map<IEnumerable<BalanceViewModel>>(await _transactionService.GetBalanceByMonthYearAsync(date));

        return GenerateResponse(transactions, HttpStatusCode.OK);
    }

    /// <summary>
    /// </summary>
    /// <remarks></remarks>
    /// <response code="201">Sucesso na operação!</response>
    /// <response code="400">Dados inconsistentes na requisição ao criar uma transação.</response>
    /// <response code="401">Usuário não autenticado.</response>
    /// <response code="500">Erro interno de servidor.</response>
    [HttpPost]
    [ProducesResponseType(typeof(TransactionViewModel), 201)]
    public async Task<ActionResult<TransactionViewModel>> Create(TransactionViewModel transactionViewModel)
    {
        if (!ModelState.IsValid) return GenerateResponse(ModelState);

        transactionViewModel.UserId = UserId;
        await _transactionService.CreateAsync(_mapper.Map<Transaction>(transactionViewModel));

        return GenerateResponse(transactionViewModel, HttpStatusCode.Created);
    }

    /// <summary>
    /// </summary>
    /// <remarks></remarks>
    /// <response code="200">Sucesso na operação!</response>
    /// <response code="400">Dados inconsistentes na requisição ao atualizar a transação.</response>
    /// <response code="401">Usuário não autenticado.</response>
    /// <response code="404">Página não encontrada.</response>
    /// <response code="500">Erro interno de servidor.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(TransactionUpdateViewModel), 200)]
    public async Task<ActionResult<TransactionUpdateViewModel>> Update(Guid id, [FromBody] TransactionUpdateViewModel transactionViewModel)
    {
        if (!ModelState.IsValid) return GenerateResponse(ModelState);

        if (id != transactionViewModel.TransactionId) return BadRequest();
        if (await GetTransactionByIdAsync(transactionViewModel.TransactionId) == null) return NotFound();

        transactionViewModel.UserId = UserId;
        await _transactionService.UpdateAsync(_mapper.Map<Transaction>(transactionViewModel));

        return GenerateResponse(transactionViewModel, HttpStatusCode.OK);
    }

    /// <summary>
    /// </summary>
    /// <remarks></remarks>
    /// <response code="204">Sucesso na operação, porém sem conteúdo de resposta.</response>
    /// <response code="400">Dados inconsistentes na requisição ao deletar uma transação.</response>
    /// <response code="401">Usuário não autenticado.</response>
    /// <response code="404">Página não encontrada.</response>
    /// <response code="500">Erro interno de servidor.</response>
    [HttpDelete("{id:guid}")]    
    public async Task<ActionResult> Delete(Guid id)
    {
        if (id == Guid.Empty) return GenerateResponse(ModelState, HttpStatusCode.BadRequest);
        if (await GetTransactionByIdAsync(id) == null) return NotFound();

        await _transactionService.DeleteAsync(id);

        return GenerateResponse(HttpStatusCode.NoContent);
    }

    private async Task<Transaction> GetTransactionByIdAsync(Guid id) => await _transactionService.GetTransactionByIdAsync(id);
}