namespace FinPlanner360.Api.Configuration;

public static class CorsConfiguration
{
    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("Dev", builder =>
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());

            options.AddPolicy("Prod", builder =>
                builder.WithOrigins("https://localhost:4200")
                    .WithMethods("OPTIONS", "GET", "POST", "PUT", "DELETE")
                    .AllowAnyHeader());
        });

        return services;
    }
}