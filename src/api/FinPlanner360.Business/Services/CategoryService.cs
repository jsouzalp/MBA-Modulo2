using FinPlanner360.Business.Interfaces.Services;
using FinPlanner360.Business.Interfaces.Repositories;
using FinPlanner360.Business.Models;
using FinPlanner360.Busines.Interfaces.Validations;

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

    public async Task CreateAsync(Category category)
    {
        if (!await _validationFactory.ValidateAsync(category))
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
        if (!await _validationFactory.ValidateAsync(category))
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