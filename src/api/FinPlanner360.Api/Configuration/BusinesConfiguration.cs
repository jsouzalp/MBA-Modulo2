using FinPlanner360.Busines.Interfaces.Services;
using FinPlanner360.Busines.Services;
using FinPlanner360.Busines.Settings;

namespace FinPlanner360.Api.Configuration;

public static class BusinesConfiguration
{
    public static IServiceCollection AddBusinesConfiguration(this IServiceCollection services, DatabaseSettings databaseSettings)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<ICategoryService, CategoryService>();

        return services;
    }
}