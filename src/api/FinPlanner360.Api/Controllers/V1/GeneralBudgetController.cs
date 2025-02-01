using AutoMapper;
using FinPlanner360.Api.ViewModels.GeneralBudget;
using FinPlanner360.Business.Interfaces.Repositories;
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
public class GeneralBudgetController : MainController
{
    private readonly IMapper _mapper;
    private readonly IGeneralBudgetService _budgetService;
    private readonly IGeneralBudgetRepository _budgetRepository;

    public GeneralBudgetController(IMapper mapper,
        IGeneralBudgetService budgetService,
        IGeneralBudgetRepository budgetRepository,
        IAppIdentityUser appIdentityUser,
        INotificationService notificationService) : base(appIdentityUser, notificationService)
    {
        _mapper = mapper;
        _budgetService = budgetService;
        _budgetRepository = budgetRepository;
    }

    /// <summary>
    /// Obtém todos os orçamentos gerais.
    /// </summary>
    /// <remarks>Retorna uma lista de orçamentos gerais cadastrados no sistema.</remarks>
    /// <response code="200">Sucesso na operação!</response>
    /// <response code="401">Usuário não autenticado.</response>
    /// <response code="404">Página não encontrada.</response>
    /// <response code="500">Erro interno de servidor.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GeneralBudgetViewModel>), 200)]
    public async Task<ActionResult<IEnumerable<GeneralBudgetViewModel>>> GetAll()
    {
        var budgets = _mapper.Map<IEnumerable<GeneralBudgetViewModel>>(await _budgetRepository.GetAllAsync());

        return GenerateResponse(budgets, HttpStatusCode.OK);
    }

    /// <summary>
    /// Cria um novo orçamento geral.
    /// </summary>
    /// <remarks>Cria um novo orçamento geral baseado nas informações fornecidas no corpo da requisição.</remarks>
    /// <response code="201">Sucesso na operação!</response>
    /// <response code="400">Dados inconsistentes na requisição ao criar um novo orçamento geral.</response>
    /// <response code="401">Usuário não autenticado.</response>
    /// <response code="500">Erro interno de servidor.</response>
    [HttpPost]
    [SwaggerOperation( Summary = "Cria um novo orçamento geral", Description = "Cria um novo orçamento geral baseado nas informações fornecidas no corpo da requisição.")]
    [ProducesResponseType(typeof(GeneralBudgetViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GeneralBudgetViewModel>> Create(GeneralBudgetViewModel generalBudgetViewModel, [FromServices] IBudgetRepository _budgetRepository)
    {
        if (!ModelState.IsValid) return GenerateResponse(ModelState);

        var budget = _mapper.Map<GeneralBudget>(generalBudgetViewModel);
        budget.UserId = UserId;
        await _budgetService.CreateAsync(budget);

        if (ValidOperation())
        {
            if (await _budgetRepository.ExistsAsync())
                Notify("Os orçamentos por categoria configurados serão ignorados, pois agora será considerado apenas o orçamento geral.", Business.Models.Enums.NotificationTypeEnum.Warning);
        }

        return GenerateResponse(generalBudgetViewModel, HttpStatusCode.Created);
    }

    /// <summary>
    /// Atualiza um orçamento geral existente.
    /// </summary>
    /// <remarks>Atualiza as informações de um orçamento geral baseado no ID fornecido.</remarks>
    /// <response code="200">Sucesso na operação!</response>
    /// <response code="400">Dados inconsistentes na requisição ao atualizar o orçamento geral.</response>
    /// <response code="401">Usuário não autenticado.</response>
    /// <response code="404">Página não encontrada.</response>
    /// <response code="500">Erro interno de servidor.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(GeneralBudgetViewModel), 200)]
    public async Task<ActionResult<GeneralBudgetViewModel>> Update(Guid id, [FromBody] GeneralBudgetViewModel generalBudgetViewModel)
    {
        if (!ModelState.IsValid) return GenerateResponse(ModelState);
        if (id != generalBudgetViewModel.GeneralBudgetId) return BadRequest();
        if (await GetGeneralBudgetByIdAsync(generalBudgetViewModel.GeneralBudgetId) == null) return NotFound();

        await _budgetService.UpdateAsync(_mapper.Map<GeneralBudget>(generalBudgetViewModel));

        return GenerateResponse(generalBudgetViewModel, HttpStatusCode.OK);
    }


    /// <summary>
    /// Exclui um orçamento geral.
    /// </summary>
    /// <remarks>Exclui um orçamento geral com base no ID fornecido.</remarks>
    /// <response code="204">Sucesso na operação, porém sem conteúdo de resposta.</response>
    /// <response code="400">Dados inconsistentes na requisição ao deletar um orçamento geral.</response>
    /// <response code="401">Usuário não autenticado.</response>
    /// <response code="404">Página não encontrada.</response>
    /// <response code="500">Erro interno de servidor.</response>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        if (id == Guid.Empty) return GenerateResponse(ModelState, HttpStatusCode.BadRequest);
        if (await GetGeneralBudgetByIdAsync(id) == null) return NotFound();

        await _budgetService.DeleteAsync(id);

        return GenerateResponse(HttpStatusCode.NoContent);
    }


    [HttpGet("exists")]
    [SwaggerOperation(Summary = "Informa se existe orçamento geral já cadastrado", Description = "Retorna status OK quando já houver orçamento cadastrado no sistema.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Exists() => await _budgetRepository.ExistsAsync() ? Ok() : NotFound();

    private async Task<GeneralBudget> GetGeneralBudgetByIdAsync(Guid id) => await _budgetRepository.GetByIdAsync(id);
}