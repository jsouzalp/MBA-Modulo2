using FinPlanner360.Busines.Interfaces.Repositories;
using FinPlanner360.Busines.Settings;
using FinPlanner360.Repositories.Contexts;
using FinPlanner360.Repositories.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FinPlanner360.Repositories.Extensions;

public static class RepositoryExtension
{
    public static IServiceCollection AddIdentityRepositories(this IServiceCollection services, DatabaseSettings databaseSettings, bool isProduction)
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

    public static IServiceCollection AddApplicationRepositories(this IServiceCollection services, DatabaseSettings databaseSettings, bool isProduction)
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

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}