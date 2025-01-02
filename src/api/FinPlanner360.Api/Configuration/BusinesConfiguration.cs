using FinPlanner360.Busines.Interfaces.Validations;
using FinPlanner360.Busines.Models.Validations;
using FinPlanner360.Business.Interfaces.Repositories;
using FinPlanner360.Business.Interfaces.Services;
using FinPlanner360.Business.Services;
using FinPlanner360.Business.Settings;
using FinPlanner360.Repositories.Contexts;
using FinPlanner360.Repositories.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
        #endregion

        #region Business injection        
        // Services
        services.AddScoped<IAppIdentityUser, AppIdentityUser>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<ICategoryService, CategoryService>();
        // others services

        // Validations
        services.AddValidatorsFromAssemblyContaining<UserValidation>();
        services.AddScoped(typeof(IValidationFactory<>), typeof(ValidationFactory<>));
        #endregion

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
                o.UseSqlite(databaseSettings.ConnectionStringFinPlanner360);
            }
        });


        return services;
    }
}