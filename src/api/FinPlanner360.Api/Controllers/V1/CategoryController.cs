using AutoMapper;
using FinPlanner360.Api.ViewModels.Category;
using FinPlanner360.Busines.Interfaces.Repositories;
using FinPlanner360.Busines.Interfaces.Services;
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

    [HttpGet("get-all")]
    public async Task<IEnumerable<CategoryViewModel>> GetAll()
    {
        return _mapper.Map<IEnumerable<CategoryViewModel>>(await _categoryRepository.GetAllAsync());
    }

    [HttpPost("create")]
    public async Task<ActionResult<CategoryViewModel>> Create(CategoryViewModel categoryViewModel)
    {
        if (!ModelState.IsValid) return GenerateResponse(ModelState);

        await _categoryService.CreateAsync(_mapper.Map<Category>(categoryViewModel));

        return GenerateResponse(categoryViewModel, HttpStatusCode.Created);
    }
}