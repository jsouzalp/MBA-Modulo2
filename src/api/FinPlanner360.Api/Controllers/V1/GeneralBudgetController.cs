using AutoMapper;
using FinPlanner360.Api.ViewModels.GeneralBudget;
using FinPlanner360.Business.Interfaces.Services;
using FinPlanner360.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FinPlanner360.Api.Controllers.V1;

[Authorize(Roles = "USER")]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[Controller]")]
public class GeneralBudgetController : MainController
{
    private readonly IMapper _mapper;
    private readonly IGeneralBudgetService _budgetService;

    public GeneralBudgetController(IMapper mapper,
        IGeneralBudgetService budgetService,
        IAppIdentityUser appIdentityUser,
        INotificationService notificationService) : base(appIdentityUser, notificationService)
    {
        _mapper = mapper;
        _budgetService = budgetService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GeneralBudgetViewModel>>> GetAll()
    {
        var budget = _mapper.Map<IEnumerable<GeneralBudgetViewModel>>(await _budgetService.GetAllAsync());
        return GenerateResponse(budget, HttpStatusCode.OK);
    }

    [HttpPost]
    public async Task<ActionResult<GeneralBudgetViewModel>> Create(GeneralBudgetViewModel generalBudgetViewModel)
    {
        if (!ModelState.IsValid) return GenerateResponse(ModelState);

        var budget = _mapper.Map<GeneralBudget>(generalBudgetViewModel);
        budget.UserId = UserId;
        await _budgetService.CreateAsync(budget);

        return GenerateResponse(generalBudgetViewModel, HttpStatusCode.Created);
    }

    [HttpPut]
    public async Task<ActionResult<GeneralBudgetViewModel>> Update(GeneralBudgetViewModel generalBudgetViewModel)
    {
        if (!ModelState.IsValid) return GenerateResponse(ModelState);

        await _budgetService.UpdateAsync(_mapper.Map<GeneralBudget>(generalBudgetViewModel));

        return GenerateResponse(generalBudgetViewModel, HttpStatusCode.OK);
    }

    [HttpDelete("{budgetId}")]
    public async Task<ActionResult> Delete(Guid budgetId)
    {
        if (budgetId == Guid.Empty) return GenerateResponse(ModelState, HttpStatusCode.BadRequest);

        await _budgetService.DeleteAsync(budgetId);

        return GenerateResponse(HttpStatusCode.NoContent);
    }
}