using AutoMapper;
using FinPlanner360.Api.ViewModels.Category;
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

    /// <summary>
    /// Obtém todas as categorias.
    /// </summary>
    /// <remarks>Busca todas as categorias cadastradas no banco de dados.</remarks>
    /// <response code="200">Sucesso na operação!</response>
    /// <response code="401">Usuário não autenticado.</response>
    /// <response code="404">Página não encontrada.</response>
    /// <response code="500">Erro interno de servidor.</response>
    [HttpGet]   
    [ProducesResponseType(typeof(List<CategoryViewModel>), 200)]
    public async Task<ActionResult<IEnumerable<CategoryViewModel>>> GetAll()
    {
        var categories = _mapper.Map<IEnumerable<CategoryViewModel>>(await _categoryRepository.GetAllAsync());

        return GenerateResponse(categories, HttpStatusCode.OK);
    }

    /// <summary>
    /// Cria uma nova categoria.
    /// </summary>
    /// <remarks>Registra uma nova categoria no banco de dados.</remarks>
    /// <response code="201">Sucesso na operação!</response>
    /// <response code="400">Dados inconsistentes na requisição ao criar a categoria.</response>
    /// <response code="401">Usuário não autenticado.</response>
    /// <response code="500">Erro interno de servidor.</response>
    [HttpPost]
    [ProducesResponseType(typeof(CategoryViewModel), 201)]
    public async Task<ActionResult<CategoryViewModel>> Create(CategoryViewModel categoryViewModel)
    {
        if (!ModelState.IsValid) return GenerateResponse(ModelState);

        categoryViewModel.UserId = UserId;
        await _categoryService.CreateAsync(_mapper.Map<Category>(categoryViewModel));

        return GenerateResponse(categoryViewModel, HttpStatusCode.Created);
    }

    /// <summary>
    /// Atualiza uma categoria existente.
    /// </summary>
    /// <remarks>Atualiza os dados de uma categoria já cadastrada no banco de dados.</remarks>
    /// <response code="200">Sucesso na operação!</response>
    /// <response code="400">Dados inconsistentes na requisição ao atualizar o orçamento.</response>
    /// <response code="401">Usuário não autenticado.</response>
    /// <response code="404">Página não encontrada.</response>
    /// <response code="500">Erro interno de servidor.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(CategoryViewModel), 200)]
    public async Task<ActionResult<CategoryUpdateViewModel>> Update(Guid id, [FromBody] CategoryUpdateViewModel categoryViewModel)
    {
        if (!ModelState.IsValid) return GenerateResponse(ModelState);
        if (id != categoryViewModel.CategoryId) return BadRequest();
        if (await GetCategoryByIdAsync(categoryViewModel.CategoryId) == null) return NotFound();

        await _categoryService.UpdateAsync(_mapper.Map<Category>(categoryViewModel));

        return GenerateResponse(categoryViewModel, HttpStatusCode.OK);
    }

    /// <summary>
    /// Exclui uma categoria.
    /// </summary>
    /// <remarks>Remove uma categoria do banco de dados pelo seu ID.</remarks>    
    /// <response code="204">Sucesso na operação, porém sem conteúdo de resposta.</response>
    /// <response code="400">Dados inconsistentes na requisição ao deletar uma categoria.</response>
    /// <response code="401">Usuário não autenticado.</response>
    /// <response code="404">Página não encontrada.</response>
    /// <response code="500">Erro interno de servidor.</response>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        if (id == Guid.Empty) return GenerateResponse(ModelState, HttpStatusCode.BadRequest);
        if (await GetCategoryByIdAsync(id) == null) return NotFound();

        await _categoryService.DeleteAsync(id);

        return GenerateResponse(HttpStatusCode.NoContent);
    }

    private async Task<Category> GetCategoryByIdAsync(Guid id) => await _categoryRepository.GetByIdAsync(id);
}