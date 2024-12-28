using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace FinPlanner360.Api.Filters;

public class ValidateAccessTokenFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();

        if (string.IsNullOrEmpty(token) || !IsValid(token))
        {
            context.Result = new UnauthorizedObjectResult(new
            {
                success = false,
                Message = "Autorização de acesso negado! Refaça seu login"
            });
        }
    }

    private bool IsValid(string token)
    {
        token = token.StartsWith("Bearer ") ? token.Substring(7) : token;

        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;
            if (expClaim == null)
                return false;

            var expirationTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim));
            if (expirationTime < DateTimeOffset.UtcNow)
                return false; 

            return true; 
        }
        catch
        {
            return false; 
        }
    }
}