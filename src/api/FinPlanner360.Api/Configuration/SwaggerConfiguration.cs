using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FinPlanner360.Api.Configuration;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Insira o token JWT desta maneira: Bearer {seu token}",
                Name = "Authorization",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            c.OperationFilter<SwaggerDefaultValues>();
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerConfig(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
    {
        app.UseSwagger();
        app.UseSwaggerUI(
            options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });
        return app;
    }

}

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    readonly IApiVersionDescriptionProvider provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }
    }

    static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo()
        {
            Title = "FinPlanner 360",
            Version = description.ApiVersion.ToString(),
            Description = "API do projeto FinPlanner do MBA DevXpert",
            Contact = new OpenApiContact() { Name = "Grupo 1"},
            License = new OpenApiLicense() { Name = "CC BY-NC-ND", Url = new Uri("https://creativecommons.org/licenses/by-nc-nd/4.0/legalcode") }
        };

        if (description.IsDeprecated)
        {
            info.Description += " Esta versão está obsoleta!";
        }

        return info;
    }
}

public class SwaggerDefaultValues : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var declaringAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true);
        var methodAttributes = context.MethodInfo.GetCustomAttributes(true);

        var isAuthorized = declaringAttributes.OfType<AuthorizeAttribute>().Any() || methodAttributes.OfType<AuthorizeAttribute>().Any();

        if (isAuthorized)
        {
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        }


        var summaryAttribute = context.MethodInfo.GetCustomAttributes(true)
            .OfType<SwaggerOperationAttribute>()
            .FirstOrDefault();

        if (summaryAttribute != null)
        {
            operation.Summary = summaryAttribute.Summary;
            operation.Description = summaryAttribute.Description;
        }
    }
}



