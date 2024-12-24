using FinPlanner360.Entities.Settings;
using FinPlanner360.Repositories.Abstractions;
using FinPlanner360.Repositories.Contexts;
using FinPlanner360.Repositories.Implementarions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FinPlanner360.Repositories.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddIdentityRepositories(this IServiceCollection services, DatabaseSettings databaseSettings)
        {
            services.AddDbContext<ApplicationDbContext>(o => o.UseSqlite(databaseSettings.ConnectionStringIdentity));
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

        public static IServiceCollection AddRepositories(this IServiceCollection services, DatabaseSettings databaseSettings)
        {
            services.AddDbContext<FinPlanner360DbContext>(o => o.UseSqlite(databaseSettings.ConnectionStringFinPlanner360));
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
