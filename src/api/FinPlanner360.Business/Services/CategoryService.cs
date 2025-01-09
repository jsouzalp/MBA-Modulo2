using FinPlanner360.Busines.Interfaces.Validations;
using FinPlanner360.Busines.Services;
using FinPlanner360.Business.Extensions;
using FinPlanner360.Business.Interfaces.Repositories;
using FinPlanner360.Business.Interfaces.Services;
using FinPlanner360.Business.Models;

namespace FinPlanner360.Business.Services;

public class CategoryService : BaseService, ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IValidationFactory<Category> _validationFactory;

    public CategoryService(IValidationFactory<Category> validationFactory,
        INotificationService notificationService,
        ICategoryRepository categoryRepository) : base(notificationService)
    {
        _validationFactory = validationFactory;
        _categoryRepository = categoryRepository;
    }

    private async Task<bool> CategoryExistsWithSameNameAsync(Guid? userId, string description)
    {
        var categories = await _categoryRepository
            .FilterAsync(c => c.Description == description && (c.UserId == null || c.UserId == userId));

        if (categories.Count != 0)
        {
            Notify("Já existe uma categoria com essa descrição.");
            return true;
        }

        return false;
    }

    public async Task CreateAsync(Category category)
    {
        if (!await _validationFactory.ValidateAsync(category))
            return;

        if (!await CategoryExistsWithSameNameAsync(category.UserId, category.Description))
        {
            await _categoryRepository.CreateAsync(category.FillAttributes());
        }
    }

    public async Task UpdateAsync(Category categoryUpdate)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryUpdate.CategoryId);
        category.Description = categoryUpdate.Description;
        category.Type = categoryUpdate.Type;

        if (!await _validationFactory.ValidateAsync(category))
            return;

        if (!await CategoryExistsWithSameNameAsync(category.UserId, category.Description))
        {
            await _categoryRepository.UpdateAsync(category);
        }
    }

    public async Task DeleteAsync(Guid categoryId)
    {
        var category = await _categoryRepository.GetCategoryById(categoryId);
        if (category == null)
        {
            Notify("Categoria não existe!");
            return;
        }

        if (category.Transactions.Any())
        {
            Notify("A categoria possui transações cadastradas!");
            return;
        }

        if (category.Budgeties.Any())
        {
            Notify("A categoria possui previsão orçamentária cadastrada!");
            return;
        }

        await _categoryRepository.RemoveAsync(categoryId);
    }
}