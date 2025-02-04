using FinPlanner360.Business.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FinPlanner360.Api.Authentication
{
    public class AppIdentityUser : IAppIdentityUser
    {
        private readonly IHttpContextAccessor _accessor;

        public AppIdentityUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public Guid GetUserId()
        {
            if (!IsAuthenticated()) return Guid.Empty;

            var claim = _accessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(claim))
                claim = _accessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            return claim is null ? Guid.Empty : Guid.Parse(claim);
        }

        public string GetUserEmail()
        {
            if (!IsAuthenticated()) return string.Empty;

            var claim = _accessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(claim))
                claim = _accessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Email)?.Value;

            return claim is null ? string.Empty : claim;
        }

        public bool IsAuthenticated()
        {
            return _accessor.HttpContext?.User.Identity is { IsAuthenticated: true };
        }
    }
}