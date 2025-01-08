using FinPlanner360.Api.Configuration;
using FinPlanner360.Api.Configuration.Swagger;
using FinPlanner360.Api.Settings;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region Settings configuration
        var configuration = builder.Configuration;
        builder.Services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));
        AppSettings appSettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
        #endregion

        #region Extended Services configuration
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddBusinesConfiguration(appSettings.DatabaseSettings, builder.Environment.IsProduction());
        builder.Services.AddApiConfiguration();
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddJwtConfiguration(appSettings.JwtSettings);
        builder.Services.AddCorsConfiguration();
        builder.Services.AddSwaggerConfiguration();
        #endregion

        var app = builder.Build();
        app.ExecuteEnvironmentConfiguration();
        app.Run();
    }
}