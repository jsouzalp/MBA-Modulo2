﻿using FinPlanner360.Api.Filters;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;

namespace FinPlanner360.Api.Configuration;

public static class SwaggerConfiguration
{
    //public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    //{
    //    services.AddSwaggerGen(c =>
    //    {
    //        c.SwaggerDoc("v1", new OpenApiInfo
    //        {
    //            Version = "v1",
    //            Title = "FinPlanner 360",
    //            Description = "API do projeto FinPlanner do MBA DevXpert",
    //            Contact = new OpenApiContact
    //            {
    //                Name = "Grupo 1"
    //            },
    //            License = new OpenApiLicense
    //            {
    //                Name = "CC BY-NC-ND",
    //                Url = new Uri("https://creativecommons.org/licenses/by-nc-nd/4.0/legalcode")
    //            }
    //        });

    //        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    //        {
    //            Description = "Token JWT: Bearer {seu token}",
    //            Name = "Authorization",
    //            Scheme = "Bearer",
    //            BearerFormat = "JWT",
    //            In = ParameterLocation.Header,
    //            Type = SecuritySchemeType.ApiKey
    //        });

    //        c.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
    //    });

    //    return services;
    //}

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

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[] {}
                    }
                });
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
        var apiDescription = context.ApiDescription;

        operation.Deprecated |= apiDescription.IsDeprecated();

        foreach (var responseType in context.ApiDescription.SupportedResponseTypes)
        {
            var responseKey = responseType.IsDefaultResponse ? "default" : responseType.StatusCode.ToString();
            var response = operation.Responses[responseKey];

            foreach (var contentType in response.Content.Keys)
                if (responseType.ApiResponseFormats.All(x => x.MediaType != contentType))
                    response.Content.Remove(contentType);
        }

        if (operation.Parameters == null)
            return;

        foreach (var parameter in operation.Parameters)
        {
            var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

            parameter.Description ??= description.ModelMetadata.Description;

            if (parameter.Schema.Default == null && description.DefaultValue != null)
            {
                var json = JsonSerializer.Serialize(description.DefaultValue, description.ModelMetadata.ModelType);
                parameter.Schema.Default = OpenApiAnyFactory.CreateFromJson(json);
            }

            parameter.Required |= description.IsRequired;
        }
    }
}

public class SwaggerAuthorizedMiddleware
{
    private readonly RequestDelegate _next;

    public SwaggerAuthorizedMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/swagger")
            && !context.User.Identity.IsAuthenticated)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        await _next.Invoke(context);
    }
}

