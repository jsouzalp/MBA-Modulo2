using FinPlanner360.Api.Configuration.Swagger;
using FinPlanner360.Data.Helpers;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace FinPlanner360.Api.Configuration;

public static class EnvironmentConfiguration
{
    public static WebApplication ExecuteEnvironmentConfiguration(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            app.UseSwaggerConfig(apiVersionDescriptionProvider);

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
            context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'; script-src 'self'");
            context.Response.Headers.Append("X-XSS-Protection", "1; mode=block"); // Proteção contra XSS
            context.Response.Headers.Append("X-Content-Type-Options", "nosniff"); // Prevenir MIME-sniffing
            await next();
        });

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }
}