using FinPlanner360.Api.Configuration;
using FinPlanner360.Business.Settings;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region Settings configuration
        var configBuilder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true);

        var configuration = configBuilder.Build();
        builder.Services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));
        AppSettings appSettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
        #endregion

        #region Extended Services configuration
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddBusinesConfiguration(appSettings.DatabaseSettings, builder.Environment.IsProduction());
        builder.Services.AddApiConfiguration();
        builder.Services.AddJwtConfiguration(appSettings.JwtSettings);
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddCorsConfiguration();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerConfiguration();
        #endregion

        var app = builder.Build();
        app.ExecuteEnvironmentConfiguration();
        app.Run();
    }
}