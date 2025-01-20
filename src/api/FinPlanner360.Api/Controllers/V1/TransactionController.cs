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

    [HttpGet("get-balance-by-month-year")]
    [SwaggerOperation(Summary = "", Description = "")]
    [ProducesResponseType(typeof(List<BalanceViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<BalanceViewModel>>> GetBalanceByMonthYear([FromQuery] DateTime date)
    {
        var transactions = _mapper.Map<IEnumerable<BalanceViewModel>>(await _transactionService.GetBalanceByMonthYearAsync(date));

        return GenerateResponse(transactions, HttpStatusCode.OK);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "", Description = "")]
    [ProducesResponseType(typeof(TransactionViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TransactionViewModel>> Create(TransactionViewModel transactionViewModel)
    {
        if (!ModelState.IsValid) return GenerateResponse(ModelState);

        transactionViewModel.UserId = UserId;
        await _transactionService.CreateAsync(_mapper.Map<Transaction>(transactionViewModel));

        return GenerateResponse(transactionViewModel, HttpStatusCode.Created);
    }

    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "", Description = "")]
    [ProducesResponseType(typeof(TransactionUpdateViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TransactionUpdateViewModel>> Update(Guid id, [FromBody] TransactionUpdateViewModel transactionViewModel)
    {
        if (!ModelState.IsValid) return GenerateResponse(ModelState);

        if (id != transactionViewModel.TransactionId) return BadRequest();
        if (await GetTransactionByIdAsync(transactionViewModel.TransactionId) == null) return NotFound();

        transactionViewModel.UserId = UserId;
        await _transactionService.UpdateAsync(_mapper.Map<Transaction>(transactionViewModel));

        return GenerateResponse(transactionViewModel, HttpStatusCode.OK);
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "", Description = "")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(Guid id)
    {
        if (id == Guid.Empty) return GenerateResponse(ModelState, HttpStatusCode.BadRequest);
        if (await GetTransactionByIdAsync(id) == null) return NotFound();

        await _transactionService.DeleteAsync(id);

        return GenerateResponse(HttpStatusCode.NoContent);
    }

    private async Task<Transaction> GetTransactionByIdAsync(Guid id) => await _transactionService.GetTransactionByIdAsync(id);
}