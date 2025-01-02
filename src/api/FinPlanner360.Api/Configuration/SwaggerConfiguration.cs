using Microsoft.OpenApi.Models;

namespace FinPlanner360.Api.Configuration;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "FinPlanner 360",
                Description = "API do projeto FinPlanner do MBA DevXpert",
                Contact = new OpenApiContact
                {
                    Name = "Grupo 1"
                },
                License = new OpenApiLicense
                {
                    Name = "CC BY-NC-ND",
                    Url = new Uri("https://creativecommons.org/licenses/by-nc-nd/4.0/legalcode")
                }
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Token JWT: Bearer {seu token}",
                Name = "Authorization",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            c.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
        });

        return services;
    }
}