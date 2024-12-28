using FinPlanner360.Busines.Interfaces.Services;
using FinPlanner360.Busines.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FinPlanner360.Busines.Services
{
    public class AuthenticationService : BaseService, IAuthenticationService
    {
        //private readonly IUserDomain _userDomain;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;

        public AuthenticationService(INotificationService notificationService,
            //IUserDomain userDomain,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IOptions<JwtSettings> jwtSettings) : base(notificationService)
        {
            //_userDomain = userDomain;
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<string> LoginUserAsync(string email, string password)
        {
            string accessToken = string.Empty;
            var resultIdentity = await _signInManager.PasswordSignInAsync(email,
                password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (resultIdentity.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(email);
                await _signInManager.SignInAsync(user, isPersistent: false);

                accessToken = await GenerateJwtAsync(user);
            }
            else
            {
                Notify("Usuário não localizado com as credenciais informadas");
            }

            return accessToken;
        }

        private async Task<string> GenerateJwtAsync(IdentityUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationInHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            return encodedToken;
        }
    }
}
