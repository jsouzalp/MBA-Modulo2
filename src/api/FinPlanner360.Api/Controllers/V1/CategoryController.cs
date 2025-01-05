using AutoMapper;
using FinPlanner360.Api.ViewModels.Category;
using FinPlanner360.Business.Interfaces.Repositories;
using FinPlanner360.Business.Interfaces.Services;
using FinPlanner360.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FinPlanner360.Api.Controllers.V1;

[Authorize(Roles = "USER")]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[Controller]")]
public class CategoryController : MainController
{
    private readonly IMapper _mapper;
    private readonly ICategoryService _categoryService;
    private readonly ICategoryRepository _categoryRepository;

    public CategoryController(IMapper mapper,
        ICategoryService categoryService,
        ICategoryRepository categoryRepository,
        IAppIdentityUser appIdentityUser,
        INotificationService notificationService) : base(appIdentityUser, notificationService)
    {
        _mapper = mapper;
        _categoryService = categoryService;
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryViewModel>>> GetAll()
    {
        var category = _mapper.Map<IEnumerable<CategoryViewModel>>(await _categoryRepository.GetAllAsync());
        return GenerateResponse(category, HttpStatusCode.OK);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryViewModel>> Create(CategoryViewModel categoryViewModel)
    {
        if (!ModelState.IsValid) return GenerateResponse(ModelState);

        categoryViewModel.UserId = UserId;
        await _categoryService.CreateAsync(_mapper.Map<Category>(categoryViewModel));

        return GenerateResponse(categoryViewModel, HttpStatusCode.Created);
    }

    [HttpPut]
    public async Task<ActionResult<CategoryUpdateViewModel>> Update(CategoryUpdateViewModel categoryViewModel)
    {
        if (!ModelState.IsValid) return GenerateResponse(ModelState);

        await _categoryService.UpdateAsync(_mapper.Map<Category>(categoryViewModel));

        return GenerateResponse(categoryViewModel, HttpStatusCode.OK);
    }

    [HttpDelete("{categoryId}")]
    public async Task<ActionResult> Delete(Guid categoryId)
    {
        if (categoryId == Guid.Empty) return GenerateResponse(ModelState, HttpStatusCode.BadRequest);

        await _categoryService.DeleteAsync(categoryId);

        return GenerateResponse(HttpStatusCode.NoContent);
    }
}