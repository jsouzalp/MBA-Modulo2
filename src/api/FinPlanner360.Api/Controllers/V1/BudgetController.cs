using AutoMapper;
using FinPlanner360.Api.ViewModels.Budget;
using FinPlanner360.Business.Interfaces.Repositories;
using FinPlanner360.Business.Interfaces.Services;
using FinPlanner360.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace FinPlanner360.Api.Controllers.V1;

[Authorize(Roles = "USER")]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[Controller]")]
public class BudgetController : MainController
{
    private readonly IMapper _mapper;
    private readonly IBudgetService _budgetService;
    private readonly IBudgetRepository _budgetRepository;

    public BudgetController(IMapper mapper,
                            IBudgetService budgetService,
                            IBudgetRepository budgetRepository,
                            IAppIdentityUser appIdentityUser,
                            INotificationService notificationService) : base(appIdentityUser, notificationService)
    {
        _mapper = mapper;
        _budgetService = budgetService;
        _budgetRepository = budgetRepository;
    }

    /// <summary>
    /// Obtém todas as entradas de orçamento.
    /// </summary>
    /// <remarks>Busca todos os orçamentos cadastrados no banco de dados.</remarks>
    /// <response code="200">Sucesso na operação!</response>
    /// <response code="401">Usuário não autenticado.</response>
    /// <response code="404">Página não encontrada.</response>
    /// <response code="500">Erro interno de servidor.</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<BudgetViewModel>), 200)]
    public async Task<ActionResult<IEnumerable<BudgetViewModel>>> GetAll()
    {
        var budgets = _mapper.Map<IEnumerable<BudgetViewModel>>(await _budgetRepository.GetAllAsync());

        return GenerateResponse(budgets, HttpStatusCode.OK);
    }

    /// <summary>
    /// Cria uma nova entrada de orçamento.
    /// </summary>
    /// <remarks>Registra um novo orçamento no banco de dados.</remarks>
    /// <response code="201">Sucesso na operação!</response>
    /// <response code="400">Dados inconsistentes na requisição ao criar o orçamento.</response>
    /// <response code="401">Usuário não autenticado.</response>
    /// <response code="500">Erro interno de servidor.</response>
    [HttpPost]
    [ProducesResponseType(typeof(BudgetViewModel), 201)]
    public async Task<ActionResult<BudgetViewModel>> Create(BudgetViewModel budgetViewModel)
    {
        if (!ModelState.IsValid) return GenerateResponse(ModelState);

        budgetViewModel.UserId = UserId;
        await _budgetService.CreateAsync(_mapper.Map<Budget>(budgetViewModel));

        return GenerateResponse(budgetViewModel, HttpStatusCode.Created);
    }

    /// <summary>
    /// Atualiza uma entrada de orçamento.
    /// </summary>
    /// <remarks>Atualiza os dados de um orçamento já existente.</remarks>
    /// <response code="200">Sucesso na operação!</response>
    /// <response code="400">Dados inconsistentes na requisição ao atualizar o orçamento.</response>
    /// <response code="401">Usuário não autenticado.</response>
    /// <response code="404">Página não encontrada.</response>
    /// <response code="500">Erro interno de servidor.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(BudgetUpdateViewModel), 200)]
    public async Task<ActionResult<BudgetUpdateViewModel>> Update(Guid id, [FromBody] BudgetUpdateViewModel budgetViewModel)
    {
        if (!ModelState.IsValid) return GenerateResponse(ModelState);
        if (id != budgetViewModel.BudgetId) return BadRequest();
        if (await _budgetService.GetBudgetByIdAsync(budgetViewModel.BudgetId) == null) return NotFound();

        await _budgetService.UpdateAsync(_mapper.Map<Budget>(budgetViewModel));

        return GenerateResponse(budgetViewModel, HttpStatusCode.OK);
    }

    /// <summary>
    /// Exclui uma entrada de orçamento.
    /// </summary>
    /// <remarks>Remove um orçamento do banco de dados pelo seu ID.</remarks>
    /// <response code="204">Sucesso na operação, porém sem conteúdo de resposta.</response>
    /// <response code="400">Dados inconsistentes na requisição ao deletar o orçamento.</response>
    /// <response code="401">Usuário não autenticado.</response>
    /// <response code="404">Página não encontrada.</response>
    /// <response code="500">Erro interno de servidor.</response>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        if (id == Guid.Empty) return GenerateResponse(ModelState, HttpStatusCode.BadRequest);
        if (await _budgetService.GetBudgetByIdAsync(id) == null) return NotFound();

        await _budgetService.DeleteAsync(id);

        return GenerateResponse(HttpStatusCode.NoContent);
    }
}