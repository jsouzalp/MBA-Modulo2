using FinPlanner360.Busines.Interfaces.Services;
using FinPlanner360.Busines.Services;
using FinPlanner360.Busines.Settings;
using FinPlanner360.Repositories.Extensions;

namespace FinPlanner360.Api.Configuration
{
    public static class BusinesConfiguration
    {
        public static IServiceCollection AddBusinesConfiguration(this IServiceCollection services, DatabaseSettings databaseSettings)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<INotificationService, NotificationService>();

            return services;
        }
    }
}
