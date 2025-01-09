using FinPlanner360.Business.Models;
using FinPlanner360.Business.Models.Enums;
using FinPlanner360.Repositories.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FinPlanner360.Repositories.Helpers;

public static class DbMigrationHelper
{
    public static async Task SeedDataAsync(WebApplication serviceScope)
    {
        var services = serviceScope.Services.CreateScope().ServiceProvider;
        await SeedDataAsync(services);
    }

    public static async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

        var applicationContext = scope.ServiceProvider.GetRequiredService<FinPlanner360DbContext>();
        var identityContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        if (env.IsDevelopment())
        {
            await applicationContext.Database.MigrateAsync();
            await identityContext.Database.MigrateAsync();
            await SeedDatabaseAsync(applicationContext, identityContext, userManager);
        }
    }

    private static async Task SeedDatabaseAsync(FinPlanner360DbContext applicationContext,
        ApplicationDbContext identityContext,
        UserManager<IdentityUser> userManager)
    {
        if (!applicationContext.Users.Any())
        {
            try
            {
                var categories = await CallDefaultCategoriesAsync(applicationContext);
                string roleId = await CallIdentityRolesAsync(identityContext);
                await CallUserConfigurationAsync(categories, applicationContext, identityContext, userManager, "André Cesconetto", "abcesconetto@gmail.com", roleId);
                await CallUserConfigurationAsync(categories, applicationContext, identityContext, userManager, "Hugo Domynique Ribeiro Nunes", "hgdmaf@gmail.com", roleId);
                await CallUserConfigurationAsync(categories, applicationContext, identityContext, userManager, "Jairo Azevedo", "jsouza.lp@gmail.com", roleId);
                await CallUserConfigurationAsync(categories, applicationContext, identityContext, userManager, "Jason Santos do Amaral", "jason.amaral@gmail.com", roleId);
                await CallUserConfigurationAsync(categories, applicationContext, identityContext, userManager, "Marco Aurelio Roque Pinto", "marco@imperiumsolucoes.com.br", roleId);
                await CallUserConfigurationAsync(categories, applicationContext, identityContext, userManager, "Pedro Otávio Gutierres", "pedro@imperiumsolucoes.com.br", roleId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    private static async Task<string> CallIdentityRolesAsync(ApplicationDbContext identityContext)
    {
        string roleId = Guid.NewGuid().ToString();
        identityContext.Roles.Add(new IdentityRole
        {
            Id = roleId,
            Name = "USER",
            NormalizedName = "USER",
            ConcurrencyStamp = DateTime.Now.ToString()
        });

        await identityContext.SaveChangesAsync();

        return roleId;
    }

    private static async Task<(Guid SalaryId, Guid HabitationId, Guid TransportId, Guid EducationId, Guid FoodId, Guid LeisureId)> CallDefaultCategoriesAsync(FinPlanner360DbContext context)
    {
        Category salary = new()
        {
            //UserId = Guid.Empty,
            CategoryId = Guid.NewGuid(),
            Description = "Salário",
            Type = CategoryTypeEnum.Income,
            CreatedDate = DateTime.Now
        };
        context.Categories.Add(salary);

        Category habitation = new()
        {
            //UserId = Guid.Empty,
            CategoryId = Guid.NewGuid(),
            Description = "Moradia",
            Type = CategoryTypeEnum.Expense,
            CreatedDate = DateTime.Now
        };
        context.Categories.Add(habitation);

        Category transport = new()
        {
            //UserId = Guid.Empty,
            CategoryId = Guid.NewGuid(),
            Description = "Transporte",
            Type = CategoryTypeEnum.Expense,
            CreatedDate = DateTime.Now
        };
        context.Categories.Add(transport);

        Category education = new()
        {
            //UserId = Guid.Empty,
            CategoryId = Guid.NewGuid(),
            Description = "Educação",
            Type = CategoryTypeEnum.Expense,
            CreatedDate = DateTime.Now
        };
        context.Categories.Add(education);

        Category food = new()
        {
            //UserId = Guid.Empty,
            CategoryId = Guid.NewGuid(),
            Description = "Alimentação",
            Type = CategoryTypeEnum.Expense,
            CreatedDate = DateTime.Now
        };
        context.Categories.Add(food);

        Category leisure = new()
        {
            //UserId = Guid.Empty,
            CategoryId = Guid.NewGuid(),
            Description = "Lazer",
            Type = CategoryTypeEnum.Expense,
            CreatedDate = DateTime.Now
        };
        context.Categories.Add(leisure);

        await context.SaveChangesAsync();

        return (salary.CategoryId, habitation.CategoryId, transport.CategoryId, education.CategoryId, food.CategoryId, leisure.CategoryId);
    }

    private static async Task CallUserConfigurationAsync((Guid SalaryId, Guid HabitationId, Guid TransportId, Guid EducationId, Guid FoodId, Guid LeisureId) categories,
        FinPlanner360DbContext applicationContext,
        ApplicationDbContext identityContext,
        UserManager<IdentityUser> userManager,
        string name,
        string email,
        string roleId)
    {
        var identityUser = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
        var result = await userManager.CreateAsync(identityUser, "Password@2024");

        if (result.Succeeded)
        {
            #region Roles

            identityContext.UserRoles.Add(new IdentityUserRole<string>()
            {
                RoleId = roleId,
                UserId = identityUser.Id.ToString()
            });

            await identityContext.SaveChangesAsync();

            #endregion Roles

            #region Data
            Guid userId = Guid.Parse(identityUser.Id);

            #region Orçamento Geral
            GeneralBudget generalBudget = new()
            {
                GeneralBudgetId = Guid.NewGuid(),
                UserId = userId,
                Amount = 10000.00m,
                CreatedDate = DateTime.Now
            };
            applicationContext.GeneralBudgets.Add(generalBudget);
            #endregion

            #region Orçamento
            Guid transportBudgetId = Guid.NewGuid();
            Budget transportBudget = new()
            {
                BudgetId = transportBudgetId,
                UserId = userId,
                Amount = 1000.00m,
                CategoryId = categories.TransportId,
                CreatedDate = DateTime.Now
            };

            Guid leisureBudgetId = Guid.NewGuid();
            Budget leisureBudget = new()
            {
                BudgetId = leisureBudgetId,
                UserId = userId,
                Amount = 2000.00m,
                CategoryId = categories.LeisureId,
                CreatedDate = DateTime.Now
            };

            Guid foodBudgetId = Guid.NewGuid();
            Budget foodBudget = new()
            {
                BudgetId = foodBudgetId,
                UserId = userId,
                Amount = 500.00m,
                CategoryId = categories.FoodId,
                CreatedDate = DateTime.Now
            };

            applicationContext.Budgets.Add(transportBudget);
            applicationContext.Budgets.Add(leisureBudget);
            applicationContext.Budgets.Add(foodBudget);
            #endregion

            #region User
            User user = new()
            {
                UserId = userId,
                Name = name,
                Email = email,
                AuthenticationId = userId,
                Transactions = new List<Transaction>()
                {
                    new()
                    {
                        TransactionId = Guid.NewGuid(),
                        UserId = userId,
                        Description = "Recebimento de Salário Mensal",
                        Amount = 10000.00m,
                        Type = TransactionTypeEnum.Income,
                        CategoryId = categories.SalaryId,
                        TransactionDate = DateTime.Now.AddDays(10).Date,
                        CreatedDate = DateTime.Now
                    },
                    new()
                    {
                        TransactionId = Guid.NewGuid(),
                        UserId = userId,
                        Description = "Pagamento de Aluguel",
                        Amount = 1000.00m,
                        Type = TransactionTypeEnum.Expense,
                        CategoryId = categories.HabitationId,
                        TransactionDate = DateTime.Now.AddDays(15).Date,
                        CreatedDate = DateTime.Now
                    },
                    new()
                    {
                        TransactionId = Guid.NewGuid(),
                        UserId = userId,
                        Description = "Uber",
                        Amount = 100.00m,
                        Type = TransactionTypeEnum.Expense,
                        CategoryId = categories.TransportId,
                        TransactionDate = DateTime.Now.Date,
                        CreatedDate = DateTime.Now
                    },
                    new()
                    {
                        TransactionId = Guid.NewGuid(),
                        UserId = userId,
                        Description = "Metro",
                        Amount = 100.00m,
                        Type = TransactionTypeEnum.Expense,
                        CategoryId = categories.TransportId,
                        TransactionDate = DateTime.Now.Date,
                        CreatedDate = DateTime.Now
                    },
                    new()
                    {
                        TransactionId = Guid.NewGuid(),
                        UserId = userId,
                        Description = "MBA Desenvolvedor.io",
                        Amount = 1000.00m,
                        Type = TransactionTypeEnum.Expense,
                        CategoryId = categories.EducationId,
                        TransactionDate = DateTime.Now.AddDays(15).Date,
                        CreatedDate = DateTime.Now
                    },
                    new()
                    {
                        TransactionId = Guid.NewGuid(),
                        UserId = userId,
                        Description = "IFood",
                        Amount = 300.00m,
                        Type = TransactionTypeEnum.Expense,
                        CategoryId = categories.FoodId,
                        TransactionDate = DateTime.Now.AddDays(15).Date,
                        CreatedDate = DateTime.Now
                    },
                    new()
                    {
                        TransactionId = Guid.NewGuid(),
                        UserId = userId,
                        Description = "Cinema",
                        Amount = 500.00m,
                        Type = TransactionTypeEnum.Expense,
                        CategoryId = categories.LeisureId,
                        TransactionDate = DateTime.Now.AddDays(20).Date,
                        CreatedDate = DateTime.Now
                    },
                }
            };

            applicationContext.Users.Add(user);
            #endregion

            #endregion Data

            await applicationContext.SaveChangesAsync();
        }
    }
}