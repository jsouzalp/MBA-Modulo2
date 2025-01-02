using FinPlanner360.Api.Settings;
using FinPlanner360.Api.ViewModels.User;
using FinPlanner360.Business.Interfaces.Repositories;
using FinPlanner360.Business.Interfaces.Services;
using FinPlanner360.Business.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinPlanner360.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[Controller]")]
public class UserController : MainController
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly AppSettings _appSettings;

    public UserController(
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager,
        IOptions<AppSettings> appSettings,
        IUserRepository userRepository,
        IAppIdentityUser appIdentityUser,
        INotificationService notificationService) : base(appIdentityUser, notificationService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _userRepository = userRepository;
        _appSettings = appSettings.Value;
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterAsync(RegisterViewModel registerViewModel)
    {
        if (!ModelState.IsValid) return GenerateResponse(ModelState);

        var identitiyUser = new IdentityUser
        {
            UserName = registerViewModel.Email,
            Email = registerViewModel.Email,
            EmailConfirmed = true
        };

        var registerResult = await _userManager.CreateAsync(identitiyUser, registerViewModel.Password);

        if (registerResult.Succeeded)
        {
            var user = new User
            {
                UserId = Guid.Parse(identitiyUser.Id),
                Email = registerViewModel.Email,
                Name = registerViewModel.Name,
                AuthenticationId = Guid.Parse(identitiyUser.Id)
            };

            await _userRepository.CreateAsync(user); // TODO: usar service ou repository direto ??

            var loginOutput = new LoginOutputViewModel
            {
                Id = user.UserId,
                Name = user.Name,
                Email = user.Email,
                AccessToken = await GenerateJwt(user.Email)
            };

            return GenerateResponse(loginOutput);
        }
        else
        {
            foreach (var error in registerResult.Errors)
            {
                Notify(error.Description);
            }
        }

        return GenerateResponse();
    }

    [HttpPost("login")]
    public async Task<ActionResult> LoginAsync(LoginViewModel loginViewModel)
    {
        if (!ModelState.IsValid) return GenerateResponse(ModelState);

        var loginResult = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, false, true);

        if (loginResult.Succeeded)
        {
            var user = await _userRepository.GetByEmailAsync(loginViewModel.Email);

            var loginOutput = new LoginOutputViewModel
            {
                Id = user.UserId,
                Name = user.Name,
                Email = user.Email,
                AccessToken = await GenerateJwt(user.Email)
            };

            return GenerateResponse(loginOutput);
        }
        else
        {
            Notify("Usuário não localizado com as credenciais informadas");
        }

        return GenerateResponse();
    }

    [HttpPost("logout")]
    public async Task LogoutAsync() => await _signInManager.SignOutAsync();

    private async Task<string> GenerateJwt(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, user.Id),
            new (ClaimTypes.Name, user.UserName),
            new (ClaimTypes.NameIdentifier, user.Id.ToString())
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