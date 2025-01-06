using AutoMapper;
using FinPlanner360.Api.ViewModels.Budget;
using FinPlanner360.Api.ViewModels.Category;
using FinPlanner360.Business.Interfaces.Services;
using FinPlanner360.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FinPlanner360.Api.Controllers.V1;

[Authorize(Roles = "USER")]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[Controller]")]
public class BudgetController : MainController
{
    private readonly IMapper _mapper;
    private readonly IBudgetService _budgetService;

    public BudgetController(IMapper mapper,
        IBudgetService budgetService,
        IAppIdentityUser appIdentityUser,
        INotificationService notificationService) : base(appIdentityUser, notificationService)
    {
        _mapper = mapper;
        _budgetService = budgetService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BudgetViewModel>>> GetAll()
    {
        var category = _mapper.Map<IEnumerable<BudgetViewModel>>(await _budgetService.GetAllAsync());
        return GenerateResponse(category, HttpStatusCode.OK);
    }

    [HttpPost]
    public async Task<ActionResult<BudgetViewModel>> Create(BudgetViewModel categoryViewModel)
    {
        if (!ModelState.IsValid) return GenerateResponse(ModelState);

        categoryViewModel.UserId = UserId;
        await _budgetService.CreateAsync(_mapper.Map<Budget>(categoryViewModel));

        return GenerateResponse(categoryViewModel, HttpStatusCode.Created);
    }

    [HttpPut]
    public async Task<ActionResult<BudgetUpdateViewModel>> Update(BudgetUpdateViewModel categoryViewModel)
    {
        if (!ModelState.IsValid) return GenerateResponse(ModelState);

        await _budgetService.UpdateAsync(_mapper.Map<Budget>(categoryViewModel));

        return GenerateResponse(categoryViewModel, HttpStatusCode.OK);
    }

    [HttpDelete("{budgetId}")]
    public async Task<ActionResult> Delete(Guid budgetId)
    {
        if (budgetId == Guid.Empty) return GenerateResponse(ModelState, HttpStatusCode.BadRequest);

        await _budgetService.DeleteAsync(budgetId);

        return GenerateResponse(HttpStatusCode.NoContent);
    }
}