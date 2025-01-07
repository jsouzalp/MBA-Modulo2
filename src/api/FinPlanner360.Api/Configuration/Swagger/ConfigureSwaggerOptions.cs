using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FinPlanner360.Api.Configuration.Swagger
{
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
                Contact = new OpenApiContact() { Name = "Grupo 1" },
                License = new OpenApiLicense() { Name = "CC BY-NC-ND", Url = new Uri("https://creativecommons.org/licenses/by-nc-nd/4.0/legalcode") }
            };

            if (description.IsDeprecated)
            {
                info.Description += " Esta versão está obsoleta!";
            }

            return info;
        }
    }
}
