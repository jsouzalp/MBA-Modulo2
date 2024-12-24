using FinPlanner360.Domains.Helpers;

namespace FinPlanner360.Api.Extensions
{
    public static class WebApplicationExtension
    {
        public static WebApplication ConfigureEnvironment(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseCors("Dev");

                DbMigrationHelper.SeedDataAsync(app).Wait();
            }
            else
            {
                app.UseCors("Prod");
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            return app;
        }
    }
}
