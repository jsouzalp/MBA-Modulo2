using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FinPlanner360.Api.Configuration.Swagger
{
    public class SwaggerDefaultValues : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var declaringAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true);
            var methodAttributes = context.MethodInfo.GetCustomAttributes(true);

            var isAuthorized = declaringAttributes.OfType<AuthorizeAttribute>().Any() || methodAttributes.OfType<AuthorizeAttribute>().Any();
            var allowAnonymous = declaringAttributes.OfType<AllowAnonymousAttribute>().Any() || methodAttributes.OfType<AllowAnonymousAttribute>().Any();

            if (isAuthorized && !allowAnonymous)
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
}
