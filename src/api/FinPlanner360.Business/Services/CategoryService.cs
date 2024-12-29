using FinPlanner360.Busines.Interfaces.Repositories;
using FinPlanner360.Busines.Interfaces.Services;
using FinPlanner360.Busines.Models;
using FinPlanner360.Busines.Models.Validations;

namespace FinPlanner360.Busines.Services;

public class CategoryService : BaseService, ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(INotificationService notificationService,
        ICategoryRepository categoryRepository) : base(notificationService)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task CreateAsync(Category category)
    {
        if (!IsValid(new CategoryValidation(), category))
            return;

        if (_categoryRepository.FilterAsync(c => c.Description == category.Description).Result.Any())
        {
            Notify("Já existe uma categoria com essa descrição.");
            return;
        }

        await _categoryRepository.CreateAsync(category);
    }

    public async Task UpdateAsync(Category category)
    {
        if (!IsValid(new CategoryValidation(), category))
            return;

        if (_categoryRepository.FilterAsync(c => c.Description == category.Description).Result.Any())
        {
            Notify("Já existe uma categoria com essa descrição.");
            return;
        }

        await _categoryRepository.UpdateAsync(category);
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
    }
}