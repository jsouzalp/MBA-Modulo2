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


    [HttpGet]
    [SwaggerOperation(Summary = "", Description = "")]
    [ProducesResponseType(typeof(IEnumerable<GeneralBudgetViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<GeneralBudgetViewModel>>> GetAll()
    {
        var budgets = _mapper.Map<IEnumerable<GeneralBudgetViewModel>>(await _budgetService.GetAllAsync());
        if (!budgets.Any()) return NotFound();

        return GenerateResponse(budgets, HttpStatusCode.OK);
    }


    [HttpPost]
    [SwaggerOperation(Summary = "", Description = "")]
    [ProducesResponseType(typeof(GeneralBudgetViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GeneralBudgetViewModel>> Create(GeneralBudgetViewModel generalBudgetViewModel)
    {
        if (!ModelState.IsValid) return GenerateResponse(ModelState);

        var budget = _mapper.Map<GeneralBudget>(generalBudgetViewModel);
        budget.UserId = UserId;
        await _budgetService.CreateAsync(budget);

        return GenerateResponse(generalBudgetViewModel, HttpStatusCode.Created);
    }


    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "", Description = "")]
    [ProducesResponseType(typeof(GeneralBudgetViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GeneralBudgetViewModel>> Update(Guid id, [FromBody] GeneralBudgetViewModel generalBudgetViewModel)
    {
        if (!ModelState.IsValid || id != generalBudgetViewModel.GeneralBudgetId) return GenerateResponse(ModelState);

        var generalBudget = await _budgetRepository.GetByIdAsync(generalBudgetViewModel.GeneralBudgetId);
        if (generalBudget == null) return NotFound();

        await _budgetService.UpdateAsync(_mapper.Map<GeneralBudget>(generalBudgetViewModel));

        return GenerateResponse(generalBudgetViewModel, HttpStatusCode.OK);
    }


    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "", Description = "")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Delete(Guid id)
    {
        if (id == Guid.Empty) return GenerateResponse(ModelState, HttpStatusCode.BadRequest);

        var generalBudget = await _budgetRepository.GetByIdAsync(id);
        if (generalBudget == null) return NotFound();


        await _budgetService.DeleteAsync(id);

        return GenerateResponse(HttpStatusCode.NoContent);
    }
}