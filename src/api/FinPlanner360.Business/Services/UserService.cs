using FinPlanner360.Busines.Services;
using FinPlanner360.Business.Interfaces.Services;
using FinPlanner360.Business.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinPlanner360.Business.Services;

public class UserService : BaseService, IUserService
{
    //private readonly IUserDomain _userDomain;
    private readonly SignInManager<IdentityUser> _signInManager;

    private readonly UserManager<IdentityUser> _userManager;
    private readonly AppSettings _appSettings;

    public UserService(INotificationService notificationService,
        //IUserDomain userDomain,
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager,
        IOptions<AppSettings> appSettings) : base(notificationService)
    {
        //_userDomain = userDomain;
        _signInManager = signInManager;
        _userManager = userManager;
        _appSettings = appSettings.Value;
    }

    public async Task<(Guid UserId, string AccessToken)> RegisterUserAsync(string email, string password)
    {
        (Guid UserId, string AccessToken) result = new();

        var user = new IdentityUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = email,
            Email = email,
            EmailConfirmed = true,
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
            new (ClaimTypes.Email, user.Email),
            new (ClaimTypes.Name, user.UserName)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.JwtSettings.Secret);

        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _appSettings.JwtSettings.Issuer,
            Audience = _appSettings.JwtSettings.Audience,
            Expires = DateTime.UtcNow.AddHours(_appSettings.JwtSettings.ExpirationInHours),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        });

        var encodedToken = tokenHandler.WriteToken(token);

        return encodedToken;
    }
}