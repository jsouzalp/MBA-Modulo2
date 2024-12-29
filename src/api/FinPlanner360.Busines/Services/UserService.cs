using FinPlanner360.Busines.Interfaces.Services;
using FinPlanner360.Busines.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinPlanner360.Busines.Services;

public class UserService : BaseService, IUserService
{
    //private readonly IUserDomain _userDomain;
    private readonly SignInManager<IdentityUser> _signInManager;

    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtSettings _jwtSettings;

    public UserService(INotificationService notificationService,
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

    public async Task<(Guid UserId, string AccessToken)> RegisterUserAsync(string email, string password)
    {
        (Guid UserId, string AccessToken) result = new();

        var user = new IdentityUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = email,
            Email = email
        };
        var registerResult = await _userManager.CreateAsync(user, password);

        if (registerResult.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "USER");
            await _signInManager.SignInAsync(user, isPersistent: false);
            result.UserId = Guid.Parse(user.Id);
            result.AccessToken = await GenerateJwtAsync(user);
        }
        else
        {
            foreach (var error in registerResult.Errors)
            {
                Notify(error.Description);
            }
        }

        return result;
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

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    private async Task<string> GenerateJwtAsync(IdentityUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, user.Id),
            new (ClaimTypes.Name, user.UserName),
            new (ClaimTypes.Email, user.Email)
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