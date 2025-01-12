using AutoMapper;
using FinPlanner360.Api.ViewModels.Category;
using FinPlanner360.Business.Extensions;
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
    [SwaggerOperation(Summary = "", Description = "")]
    [ProducesResponseType(typeof(List<CategoryViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<CategoryViewModel>>> GetAll()
    {
        var categories = _mapper.Map<IEnumerable<CategoryViewModel>>(await _categoryRepository.GetAllAsync());

        return GenerateResponse(categories, HttpStatusCode.OK);
    }


    [HttpPost]
    [SwaggerOperation(Summary = "", Description = "")]
    [ProducesResponseType(typeof(CategoryViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CategoryViewModel>> Create(CategoryViewModel categoryViewModel)
    {
        if (!ModelState.IsValid) return GenerateResponse(ModelState);

        categoryViewModel.UserId = UserId;
        await _categoryService.CreateAsync(_mapper.Map<Category>(categoryViewModel));

        return GenerateResponse(categoryViewModel, HttpStatusCode.Created);
    }


    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "", Description = "")]
    [ProducesResponseType(typeof(CategoryViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryUpdateViewModel>> Update(Guid id, [FromBody] CategoryUpdateViewModel categoryViewModel)
    {
        if (!ModelState.IsValid) return GenerateResponse(ModelState);
        if (id != categoryViewModel.CategoryId) return BadRequest();
        if (await GetCategoryByIdAsync(categoryViewModel.CategoryId) == null) return NotFound();

        await _categoryService.UpdateAsync(_mapper.Map<Category>(categoryViewModel));

        return GenerateResponse(categoryViewModel, HttpStatusCode.OK);
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
        if (await GetCategoryByIdAsync(id) == null) return NotFound();

        await _categoryService.DeleteAsync(id);

        return GenerateResponse(HttpStatusCode.NoContent);
    }

    private async Task<Category> GetCategoryByIdAsync(Guid id) => await _categoryRepository.GetByIdAsync(id);
}