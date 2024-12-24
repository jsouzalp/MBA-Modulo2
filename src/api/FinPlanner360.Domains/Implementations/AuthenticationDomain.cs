using FinPlanner360.Domains.Abstractions;
using FinPlanner360.Domains.Entities;
using FinPlanner360.Entities;
using FinPlanner360.Entities.Authentication;
using FinPlanner360.Entities.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinPlanner360.Domains.Implementations
{
    public class AuthenticationDomain : IAuthenticationDomain
    {
        private readonly IUserDomain _userDomain;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;

        public AuthenticationDomain(IUserDomain userDomain,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IOptions<JwtSettings> jwtSettings)
        {
            _userDomain = userDomain;
            //_signInManager = signInManager;
            //_userManager = userManager;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<DomainOutput<LoginOutput>> LoginUserAsync(bool generateToken, DomainInput<LoginInput> loginUser)
        {
            DomainOutput<LoginOutput> result = new();
            var resultIdentity = await _signInManager.PasswordSignInAsync(loginUser.Input.Email,
                loginUser.Input.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (resultIdentity.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(loginUser.Input.Email);
                await _signInManager.SignInAsync(user, isPersistent: false);

                result.Message = "Login realizado com sucesso";
                result.Output = new LoginOutput()
                {
                    Id = Guid.Parse(user.Id),
                    AccessToken = generateToken ? await GenerateJwtAsync(user) : string.Empty
                };
            }
            else
            {
                result.Errors = new List<ErrorBase>()
                {
                    new()
                    {
                        Code = "1000",
                        Message = "Usuário não localizado com as credenciais informadas"
                    }
                };
            }

            return result;
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
