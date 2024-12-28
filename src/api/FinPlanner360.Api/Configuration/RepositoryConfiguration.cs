using FinPlanner360.Busines.Settings;
using FinPlanner360.Repositories.Extensions;

namespace FinPlanner360.Api.Configuration;

public static class RepositoryConfiguration
{
    public static IServiceCollection AddRepositoryConfiguration(this IServiceCollection services, DatabaseSettings databaseSettings, bool isProduction)
    {
        services.AddIdentityRepositories(databaseSettings, isProduction);
        services.AddApplicationRepositories(databaseSettings, isProduction);

        return services;
    }
}