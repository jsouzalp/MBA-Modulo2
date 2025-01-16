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

            DateTime baseDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            User user = new()
            {
                UserId = userId,
                Name = name,
                Email = email,
                AuthenticationId = userId,
                Transactions = new List<Transaction>()
                {
                    // -12 meses
                    CreateTransaction(userId, "Recebimento de Salário Mensal", 5000.00m, TransactionTypeEnum.Income, categories.SalaryId, baseDate.AddDays(10).AddMonths(-12).Date),
                    CreateTransaction(userId, "Pagamento de Aluguel", 5000.00m, TransactionTypeEnum.Expense, categories.HabitationId, baseDate.AddDays(15).AddMonths(-12).Date),
                    CreateTransaction(userId, "Uber", 100.00m, TransactionTypeEnum.Expense, categories.TransportId, baseDate.AddMonths(-12).Date),
                    CreateTransaction(userId, "Metro", 100.00m, TransactionTypeEnum.Expense, categories.TransportId, baseDate.AddMonths(-12).Date),
                    CreateTransaction(userId, "MBA Desenvolvedor.io", 1000.00m, TransactionTypeEnum.Expense, categories.EducationId, baseDate.AddDays(15).AddMonths(-12).Date),
                    CreateTransaction(userId, "IFood", 300.00m, TransactionTypeEnum.Expense, categories.FoodId, baseDate.AddDays(20).AddMonths(-12).Date),
                    CreateTransaction(userId, "Cinema", 500.00m, TransactionTypeEnum.Expense, categories.LeisureId, baseDate.AddDays(7).AddMonths(-12).Date),

                    // -11 meses
                    CreateTransaction(userId, "Recebimento de Salário Mensal", 8000.00m, TransactionTypeEnum.Income, categories.SalaryId, baseDate.AddDays(10).AddMonths(-11).Date),
                    CreateTransaction(userId, "Pagamento de Aluguel", 5500.00m, TransactionTypeEnum.Expense, categories.HabitationId, baseDate.AddDays(15).AddMonths(-11).Date),
                    CreateTransaction(userId, "Uber", 150.00m, TransactionTypeEnum.Expense, categories.TransportId, baseDate.AddMonths(-11).Date),
                    CreateTransaction(userId, "Metro", 150.00m, TransactionTypeEnum.Expense, categories.TransportId, baseDate.AddMonths(-11).Date),
                    CreateTransaction(userId, "MBA Desenvolvedor.io", 1500.00m, TransactionTypeEnum.Expense, categories.EducationId, baseDate.AddDays(15).AddMonths(-11).Date),
                    CreateTransaction(userId, "IFood", 350.00m, TransactionTypeEnum.Expense, categories.FoodId, baseDate.AddDays(20).AddMonths(-11).Date),
                    CreateTransaction(userId, "Cinema", 550.00m, TransactionTypeEnum.Expense, categories.LeisureId, baseDate.AddDays(7).AddMonths(-11).Date),

                    // -10 meses
                    CreateTransaction(userId, "Recebimento de Salário Mensal", 10000.00m, TransactionTypeEnum.Income, categories.SalaryId, baseDate.AddDays(10).AddMonths(-10).Date),
                    CreateTransaction(userId, "Pagamento de Aluguel", 5000.00m, TransactionTypeEnum.Expense, categories.HabitationId, baseDate.AddDays(15).AddMonths(-10).Date),
                    CreateTransaction(userId, "Uber", 100.00m, TransactionTypeEnum.Expense, categories.TransportId, baseDate.AddMonths(-10).Date),
                    CreateTransaction(userId, "Metro", 100.00m, TransactionTypeEnum.Expense, categories.TransportId, baseDate.AddMonths(-10).Date),
                    CreateTransaction(userId, "MBA Desenvolvedor.io", 1200.00m, TransactionTypeEnum.Expense, categories.EducationId, baseDate.AddDays(15).AddMonths(-10).Date),
                    CreateTransaction(userId, "IFood", 300.00m, TransactionTypeEnum.Expense, categories.FoodId, baseDate.AddDays(20).AddMonths(-10).Date),
                    CreateTransaction(userId, "Cinema", 500.00m, TransactionTypeEnum.Expense, categories.LeisureId, baseDate.AddDays(7).AddMonths(-10).Date),

                    // -9 meses
                    CreateTransaction(userId, "Recebimento de Salário Mensal", 12000.00m, TransactionTypeEnum.Income, categories.SalaryId, baseDate.AddDays(10).AddMonths(-9).Date),
                    CreateTransaction(userId, "Pagamento de Aluguel", 5500.00m, TransactionTypeEnum.Expense, categories.HabitationId, baseDate.AddDays(15).AddMonths(-9).Date),
                    CreateTransaction(userId, "Uber", 150.00m, TransactionTypeEnum.Expense, categories.TransportId, baseDate.AddMonths(-9).Date),
                    CreateTransaction(userId, "Metro", 150.00m, TransactionTypeEnum.Expense, categories.TransportId, baseDate.AddMonths(-9).Date),
                    CreateTransaction(userId, "MBA Desenvolvedor.io", 1500.00m, TransactionTypeEnum.Expense, categories.EducationId, baseDate.AddDays(15).AddMonths(-9).Date),
                    CreateTransaction(userId, "IFood", 350.00m, TransactionTypeEnum.Expense, categories.FoodId, baseDate.AddDays(20).AddMonths(-9).Date),
                    CreateTransaction(userId, "Cinema", 550.00m, TransactionTypeEnum.Expense, categories.LeisureId, baseDate.AddDays(7).AddMonths(-9).Date),

                    // -8 meses
                    CreateTransaction(userId, "Recebimento de Salário Mensal", 10000.00m, TransactionTypeEnum.Income, categories.SalaryId, baseDate.AddDays(10).AddMonths(-8).Date),
                    CreateTransaction(userId, "Pagamento de Aluguel", 5000.00m, TransactionTypeEnum.Expense, categories.HabitationId, baseDate.AddDays(15).AddMonths(-8).Date),
                    CreateTransaction(userId, "Uber", 100.00m, TransactionTypeEnum.Expense, categories.TransportId, baseDate.AddMonths(-8).Date),
                    CreateTransaction(userId, "Metro", 100.00m, TransactionTypeEnum.Expense, categories.TransportId, baseDate.AddMonths(-8).Date),
                    CreateTransaction(userId, "MBA Desenvolvedor.io", 1200.00m, TransactionTypeEnum.Expense, categories.EducationId, baseDate.AddDays(15).AddMonths(-8).Date),
                    CreateTransaction(userId, "IFood", 300.00m, TransactionTypeEnum.Expense, categories.FoodId, baseDate.AddDays(20).AddMonths(-8).Date),
                    CreateTransaction(userId, "Cinema", 500.00m, TransactionTypeEnum.Expense, categories.LeisureId, baseDate.AddDays(7).AddMonths(-8).Date),

                    // -7 meses
                    CreateTransaction(userId, "Recebimento de Salário Mensal", 12000.00m, TransactionTypeEnum.Income, categories.SalaryId, baseDate.AddDays(10).AddMonths(-7).Date),
                    CreateTransaction(userId, "Pagamento de Aluguel", 1500.00m, TransactionTypeEnum.Expense, categories.HabitationId, baseDate.AddDays(15).AddMonths(-7).Date),
                    CreateTransaction(userId, "Uber", 150.00m, TransactionTypeEnum.Expense, categories.TransportId, baseDate.AddMonths(-7).Date),
                    CreateTransaction(userId, "Metro", 150.00m, TransactionTypeEnum.Expense, categories.TransportId, baseDate.AddMonths(-7).Date),
                    CreateTransaction(userId, "MBA Desenvolvedor.io", 1500.00m, TransactionTypeEnum.Expense, categories.EducationId, baseDate.AddDays(15).AddMonths(-7).Date),
                    CreateTransaction(userId, "IFood", 350.00m, TransactionTypeEnum.Expense, categories.FoodId, baseDate.AddDays(20).AddMonths(-7).Date),
                    CreateTransaction(userId, "Cinema", 550.00m, TransactionTypeEnum.Expense, categories.LeisureId, baseDate.AddDays(7).AddMonths(-7).Date),

                    // -6 meses
                    CreateTransaction(userId, "Recebimento de Salário Mensal", 10000.00m, TransactionTypeEnum.Income, categories.SalaryId, baseDate.AddDays(10).AddMonths(-6).Date),
                    CreateTransaction(userId, "Pagamento de Aluguel", 1000.00m, TransactionTypeEnum.Expense, categories.HabitationId, baseDate.AddDays(15).AddMonths(-6).Date),
                    CreateTransaction(userId, "Uber", 100.00m, TransactionTypeEnum.Expense, categories.TransportId, baseDate.AddMonths(-6).Date),
                    CreateTransaction(userId, "Metro", 100.00m, TransactionTypeEnum.Expense, categories.TransportId, baseDate.AddMonths(-6).Date),
                    CreateTransaction(userId, "MBA Desenvolvedor.io", 1200.00m, TransactionTypeEnum.Expense, categories.EducationId, baseDate.AddDays(15).AddMonths(-6).Date),
                    CreateTransaction(userId, "IFood", 300.00m, TransactionTypeEnum.Expense, categories.FoodId, baseDate.AddDays(20).AddMonths(-6).Date),
                    CreateTransaction(userId, "Cinema", 500.00m, TransactionTypeEnum.Expense, categories.LeisureId, baseDate.AddDays(7).AddMonths(-6).Date),

                    // -5 meses
                    CreateTransaction(userId, "Recebimento de Salário Mensal", 15000.00m, TransactionTypeEnum.Income, categories.SalaryId, baseDate.AddDays(10).AddMonths(-5).Date),
                    CreateTransaction(userId, "Pagamento de Aluguel", 3500.00m, TransactionTypeEnum.Expense, categories.HabitationId, baseDate.AddDays(15).AddMonths(-5).Date),
                    CreateTransaction(userId, "Uber", 1500.00m, TransactionTypeEnum.Expense, categories.TransportId, baseDate.AddMonths(-5).Date),
                    CreateTransaction(userId, "Metro", 150.00m, TransactionTypeEnum.Expense, categories.TransportId, baseDate.AddMonths(-5).Date),
                    CreateTransaction(userId, "MBA Desenvolvedor.io", 1500.00m, TransactionTypeEnum.Expense, categories.EducationId, baseDate.AddDays(15).AddMonths(-5).Date),
                    CreateTransaction(userId, "IFood", 350.00m, TransactionTypeEnum.Expense, categories.FoodId, baseDate.AddDays(20).AddMonths(-5).Date),
                    CreateTransaction(userId, "Cinema", 550.00m, TransactionTypeEnum.Expense, categories.LeisureId, baseDate.AddDays(7).AddMonths(-5).Date),

                    // -4 meses
                    CreateTransaction(userId, "Recebimento de Salário Mensal", 10000.00m, TransactionTypeEnum.Income, categories.SalaryId, baseDate.AddDays(10).AddMonths(-4).Date),
                    CreateTransaction(userId, "Pagamento de Aluguel", 2000.00m, TransactionTypeEnum.Expense, categories.HabitationId, baseDate.AddDays(15).AddMonths(-4).Date),
                    CreateTransaction(userId, "Uber", 1000.00m, TransactionTypeEnum.Expense, categories.TransportId, baseDate.AddMonths(-4).Date),
                    CreateTransaction(userId, "Metro", 100.00m, TransactionTypeEnum.Expense, categories.TransportId, baseDate.AddMonths(-4).Date),
                    CreateTransaction(userId, "MBA Desenvolvedor.io", 1200.00m, TransactionTypeEnum.Expense, categories.EducationId, baseDate.AddDays(15).AddMonths(-4).Date),
                    CreateTransaction(userId, "IFood", 300.00m, TransactionTypeEnum.Expense, categories.FoodId, baseDate.AddDays(20).AddMonths(-4).Date),
                    CreateTransaction(userId, "Cinema", 500.00m, TransactionTypeEnum.Expense, categories.LeisureId, baseDate.AddDays(7).AddMonths(-4).Date),

                    // -3 meses
                    CreateTransaction(userId, "Recebimento de Salário Mensal", 15000.00m, TransactionTypeEnum.Income, categories.SalaryId, baseDate.AddDays(10).AddMonths(-3).Date),
                    CreateTransaction(userId, "Pagamento de Aluguel", 1500.00m, TransactionTypeEnum.Expense, categories.HabitationId, baseDate.AddDays(15).AddMonths(-3).Date),
                    CreateTransaction(userId, "Uber", 150.00m, TransactionTypeEnum.Expense, categories.TransportId, baseDate.AddMonths(-3).Date),
                    CreateTransaction(userId, "Metro", 150.00m, TransactionTypeEnum.Expense, categories.TransportId, baseDate.AddMonths(-3).Date),
                    CreateTransaction(userId, "MBA Desenvolvedor.io", 1500.00m, TransactionTypeEnum.Expense, categories.EducationId, baseDate.AddDays(15).AddMonths(-3).Date),
                    CreateTransaction(userId, "IFood", 350.00m, TransactionTypeEnum.Expense, categories.FoodId, baseDate.AddDays(20).AddMonths(-3).Date),
                    CreateTransaction(userId, "Cinema", 550.00m, TransactionTypeEnum.Expense, categories.LeisureId, baseDate.AddDays(7).AddMonths(-3).Date),

                    // -2 meses
                    CreateTransaction(userId, "Recebimento de Salário Mensal", 10000.00m, TransactionTypeEnum.Income, categories.SalaryId, baseDate.AddDays(10).AddMonths(-2).Date),
                    CreateTransaction(userId, "Pagamento de Aluguel", 1000.00m, TransactionTypeEnum.Expense, categories.HabitationId, baseDate.AddDays(15).AddMonths(-2).Date),
                    CreateTransaction(userId, "Uber", 100.00m, TransactionTypeEnum.Expense, categories.TransportId, baseDate.AddMonths(-2).Date),
                    CreateTransaction(userId, "Metro", 100.00m, TransactionTypeEnum.Expense, categories.TransportId, baseDate.AddMonths(-2).Date),
                    CreateTransaction(userId, "MBA Desenvolvedor.io", 1200.00m, TransactionTypeEnum.Expense, categories.EducationId, baseDate.AddDays(15).AddMonths(-2).Date),
                    CreateTransaction(userId, "IFood", 300.00m, TransactionTypeEnum.Expense, categories.FoodId, baseDate.AddDays(20).AddMonths(-2).Date),
                    CreateTransaction(userId, "Cinema", 500.00m, TransactionTypeEnum.Expense, categories.LeisureId, baseDate.AddDays(7).AddMonths(-2).Date),

                    // -1 meses
                    CreateTransaction(userId, "Recebimento de Salário Mensal", 15000.00m, TransactionTypeEnum.Income, categories.SalaryId, baseDate.AddDays(10).AddMonths(-1).Date),
                    CreateTransaction(userId, "Pagamento de Aluguel", 1500.00m, TransactionTypeEnum.Expense, categories.HabitationId, baseDate.AddDays(15).AddMonths(-1).Date),
                    CreateTransaction(userId, "Uber", 150.00m, TransactionTypeEnum.Expense, categories.TransportId, baseDate.AddMonths(-1).Date),
                    CreateTransaction(userId, "Metro", 150.00m, TransactionTypeEnum.Expense, categories.TransportId, baseDate.AddMonths(-1).Date),
                    CreateTransaction(userId, "MBA Desenvolvedor.io", 1500.00m, TransactionTypeEnum.Expense, categories.EducationId, baseDate.AddDays(15).AddMonths(-1).Date),
                    CreateTransaction(userId, "IFood", 350.00m, TransactionTypeEnum.Expense, categories.FoodId, baseDate.AddDays(20).AddMonths(-1).Date),
                    CreateTransaction(userId, "Cinema", 550.00m, TransactionTypeEnum.Expense, categories.LeisureId, baseDate.AddDays(7).AddMonths(-1).Date),

                    // Mês Atual
                    CreateTransaction(userId, "Recebimento de Salário Mensal", 10000.00m, TransactionTypeEnum.Income, categories.SalaryId, baseDate.AddDays(10).Date),
                    CreateTransaction(userId, "Pagamento de Aluguel", 1000.00m, TransactionTypeEnum.Expense, categories.HabitationId, baseDate.AddDays(15).Date),
                    CreateTransaction(userId, "Uber", 100.00m, TransactionTypeEnum.Expense, categories.TransportId, baseDate.Date),
                    CreateTransaction(userId, "Metro", 100.00m, TransactionTypeEnum.Expense, categories.TransportId, baseDate.Date),
                    CreateTransaction(userId, "MBA Desenvolvedor.io", 1200.00m, TransactionTypeEnum.Expense, categories.EducationId, baseDate.AddDays(15).Date),
                    CreateTransaction(userId, "IFood", 300.00m, TransactionTypeEnum.Expense, categories.FoodId, baseDate.AddDays(20).Date),
                    CreateTransaction(userId, "Cinema", 500.00m, TransactionTypeEnum.Expense, categories.LeisureId, baseDate.AddDays(7).Date),

                    // +1 mes
                    CreateTransaction(userId, "Recebimento de Salário Mensal", 15000.00m, TransactionTypeEnum.Income, categories.SalaryId, baseDate.AddDays(10).AddMonths(1).Date),
                    CreateTransaction(userId, "Pagamento de Aluguel", 1500.00m, TransactionTypeEnum.Expense, categories.HabitationId, baseDate.AddDays(15).AddMonths(1).Date),
                    CreateTransaction(userId, "Uber", 150.00m, TransactionTypeEnum.Expense, categories.TransportId, baseDate.AddMonths(1).Date),
                    CreateTransaction(userId, "Metro", 150.00m, TransactionTypeEnum.Expense, categories.TransportId, baseDate.AddMonths(1).Date),
                    CreateTransaction(userId, "MBA Desenvolvedor.io", 1500.00m, TransactionTypeEnum.Expense, categories.EducationId, baseDate.AddDays(15).AddMonths(1).Date),
                    CreateTransaction(userId, "IFood", 350.00m, TransactionTypeEnum.Expense, categories.FoodId, baseDate.AddDays(20).AddMonths(1).Date),
                    CreateTransaction(userId, "Cinema", 550.00m, TransactionTypeEnum.Expense, categories.LeisureId, baseDate.AddDays(7).AddMonths(1).Date),
                }
            };

            applicationContext.Users.Add(user);
            #endregion

            #endregion Data

            await applicationContext.SaveChangesAsync();
        }
    }

    private static Transaction CreateTransaction(Guid userId, string description, decimal amount, TransactionTypeEnum type, Guid categoryId, DateTime transactionDate) 
        => new Transaction()
        {
            TransactionId = Guid.NewGuid(),
            UserId = userId,
            Description = description,
            Amount = amount,
            Type = type,
            CategoryId = categoryId,
            TransactionDate = transactionDate,
            CreatedDate = DateTime.Now
        };
}