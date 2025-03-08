using FinPlanner360.Api.Authentication;
using FinPlanner360.Api.Settings;
using FinPlanner360.Busines.Interfaces.Validations;
using FinPlanner360.Busines.Models.Validations;
using FinPlanner360.Business.Interfaces.Repositories;
using FinPlanner360.Business.Interfaces.Services;
using FinPlanner360.Business.Services;
using FinPlanner360.Data.Contexts;
using FinPlanner360.Data.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Globalization;

namespace FinPlanner360.Api.Configuration;

public static class BusinesConfiguration
{
    public static IServiceCollection AddBusinesConfiguration(this IServiceCollection services, DatabaseSettings databaseSettings, bool isProduction)
    {
        services.AddIdentityRepositories(databaseSettings, isProduction);
        services.AddApplicationRepositories(databaseSettings, isProduction);

        #region Repositories injection

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IBudgetRepository, BudgetRepository>();
        services.AddScoped<IGeneralBudgetRepository, GeneralBudgetRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        #endregion Repositories injection

        #region Business injection

        // Services
        services.AddScoped<IAppIdentityUser, AppIdentityUser>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IBudgetService, BudgetService>();
        services.AddScoped<IGeneralBudgetService, GeneralBudgetService>();
        services.AddScoped<ITransactionService, TransactionService>();
        // others services

        // Validations
        services.AddValidatorsFromAssemblyContaining<UserValidation>();
        services.AddScoped(typeof(IValidationFactory<>), typeof(ValidationFactory<>));

        #endregion Business injection

        //Swagger
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, Swagger.ConfigureSwaggerOptions>();

        return services;
    }

    private static IServiceCollection AddIdentityRepositories(this IServiceCollection services, DatabaseSettings databaseSettings, bool isProduction)
    {
        services.AddDbContext<ApplicationDbContext>(o =>
        {
            if (isProduction)
            {
                o.UseSqlServer(databaseSettings.ConnectionStringFinPlanner360);
            }
            else
            {
                o.UseSqlite(databaseSettings.ConnectionStringFinPlanner360);
            }
        });

        services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 0;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddRoles<IdentityRole>()
        .AddDefaultTokenProviders();

        return services;
    }

    private static IServiceCollection AddApplicationRepositories(this IServiceCollection services, DatabaseSettings databaseSettings, bool isProduction)
    {
        services.AddDbContext<FinPlanner360DbContext>(o =>
        {
            if (isProduction)
            {
                o.UseSqlServer(databaseSettings.ConnectionStringFinPlanner360);
            }
            else
            {
                var connection = new SqliteConnection(databaseSettings.ConnectionStringFinPlanner360);
                connection.CreateCollation("LATIN1_GENERAL_CI_AI", (x, y) =>
                {
                    if (x == null && y == null) return 0;
                    if (x == null) return -1;
                    if (y == null) return 1;

                    // Comparação ignorando maiúsculas/minúsculas e acentos
                    return string.Compare(x, y, CultureInfo.CurrentCulture, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace);
                });

                o.UseSqlite(connection);
            }
        });

        return services;
    }
}