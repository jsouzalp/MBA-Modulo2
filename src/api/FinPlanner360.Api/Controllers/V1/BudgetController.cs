using AutoMapper;
using FinPlanner360.Api.ViewModels.Budget;
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

    [HttpGet]
    [SwaggerOperation(Summary = "Obtém todas as entradas de orçamento.", Description = "Busca todos os orçamentos cadastrados no banco de dados.")]
    [ProducesResponseType(typeof(List<BudgetViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<BudgetViewModel>>> GetAll()
    {
        var budgets = _mapper.Map<IEnumerable<BudgetViewModel>>(await _budgetRepository.GetAllAsync());

        return GenerateResponse(budgets, HttpStatusCode.OK);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Cria uma nova entrada de orçamento.",Description = "Registra um novo orçamento no banco de dados.")]
    [ProducesResponseType(typeof(BudgetViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<BudgetViewModel>> Create(BudgetViewModel budgetViewModel)
    {
        if (!ModelState.IsValid) return GenerateResponse(ModelState);

        budgetViewModel.UserId = UserId;
        await _budgetService.CreateAsync(_mapper.Map<Budget>(budgetViewModel));

        return GenerateResponse(budgetViewModel, HttpStatusCode.Created);
    }

    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "Atualiza uma entrada de orçamento.", Description = "Atualiza os dados de um orçamento já existente.")]
    [ProducesResponseType(typeof(BudgetUpdateViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BudgetUpdateViewModel>> Update(Guid id, [FromBody] BudgetUpdateViewModel budgetViewModel)
    {
        if (!ModelState.IsValid) return GenerateResponse(ModelState);
        if (id != budgetViewModel.BudgetId) return BadRequest();
        if (await _budgetService.GetBudgetByIdAsync(budgetViewModel.BudgetId) == null) return NotFound();

        await _budgetService.UpdateAsync(_mapper.Map<Budget>(budgetViewModel));

        return GenerateResponse(budgetViewModel, HttpStatusCode.OK);
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Exclui uma entrada de orçamento.", Description = "Remove um orçamento do banco de dados pelo seu ID.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(Guid id)
    {
        if (id == Guid.Empty) return GenerateResponse(ModelState, HttpStatusCode.BadRequest);
        if (await _budgetService.GetBudgetByIdAsync(id) == null) return NotFound();

        await _budgetService.DeleteAsync(id);

        return GenerateResponse(HttpStatusCode.NoContent);
    }
}