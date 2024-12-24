using FinPlanner360.Domains.Abstractions;
using FinPlanner360.Domains.Implementations;
using FinPlanner360.Entities.Settings;
using FinPlanner360.Repositories.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FinPlanner360.Domains.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDomains(this IServiceCollection services, DatabaseSettings databaseSettings)
        {
            services.AddIdentityRepositories(databaseSettings);
            services.AddRepositories(databaseSettings);
            services.AddScoped<IAuthenticationDomain, AuthenticationDomain>();
            services.AddScoped<IUserDomain, UserDomain>();

            return services;
        }

        public static IServiceCollection AddJwtConfiguration(this IServiceCollection services, JwtSettings jwtSettings)
        {
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidIssuer = jwtSettings.Issuer
                };
            });

            return services;
        }
    }
}
