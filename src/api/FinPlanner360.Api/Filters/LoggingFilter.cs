using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;


namespace FinPlanner360.Api.Filters;

public class LoggingFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        token = token.StartsWith("Bearer ") ? token.Substring(7) : token;

        if (!string.IsNullOrWhiteSpace(token))
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            string userId = jwtToken.Claims.FirstOrDefault(x => string.Equals(x.Type, JwtRegisteredClaimNames.Sub, StringComparison.OrdinalIgnoreCase))?.Value;
            string email = jwtToken.Claims.FirstOrDefault(x => string.Equals(x.Type, JwtRegisteredClaimNames.Email, StringComparison.OrdinalIgnoreCase))?.Value;
            context.HttpContext.Items[JwtRegisteredClaimNames.Sub] = userId;
            context.HttpContext.Items[JwtRegisteredClaimNames.Email] = email;
        }
    }
}
