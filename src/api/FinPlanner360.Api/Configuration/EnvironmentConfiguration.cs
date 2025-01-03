using FinPlanner360.Repositories.Helpers;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace FinPlanner360.Api.Configuration;

public static class EnvironmentConfiguration
{
    public static WebApplication ExecuteEnvironmentConfiguration(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(
            options =>
            {
                var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });

            app.UseCors("Dev");

            DbMigrationHelper.SeedDataAsync(app).Wait();
        }
        else
        {
            app.UseCors("Prod");
        }

        app.UseStaticFiles();
        app.UseHttpsRedirection();

        app.UseHsts();

        app.Use(async (context, next) =>
        {
            context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; script-src 'self'");
            context.Response.Headers.Add("X-XSS-Protection", "1; mode=block"); // Proteção contra XSS
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff"); // Prevenir MIME-sniffing
            await next();
        });

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }
}