using FinPlanner360.Api.ViewModels.Authentication;
using FinPlanner360.Busines.Interfaces.Repositories;
using FinPlanner360.Busines.Interfaces.Services;
using FinPlanner360.Busines.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace FinPlanner360.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Area("auth")]
    [Route("api/v{version:apiVersion}/[area]")]
    public class AuthenticationController : MainController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserRepository _userRepository;

        public AuthenticationController(IAuthenticationService authenticationService,
            IUserRepository userRepository,
            INotificationService notificationService) : base(notificationService)
        {
            _authenticationService = authenticationService;
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync(RegisterViewModel registerViewModel)
        {
            LoginOutputViewModel result = null;
            if (!ModelState.IsValid) return GenerateResponse(ModelState);

            var accessToken = await _authenticationService.RegisterUserAsync(registerViewModel.Email, registerViewModel.Password);

            if (accessToken.UserId != Guid.Empty)
            {
                // TODO :: Poderia ser refatorado de forma a ir para o autoMapper?
                await _userRepository.CreateAsync(new User
                {
                    UserId = accessToken.UserId,
                    Email = registerViewModel.Email,
                    Name = registerViewModel.Name,
                    AuthenticationId = accessToken.UserId
                });

                result = new LoginOutputViewModel();
                result.Id = accessToken.UserId;
                result.Name = registerViewModel.Name;
                result.Email = registerViewModel.Email;
                result.AccessToken = accessToken.AccessToken;
            }

            return GenerateResponse(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync(LoginViewModel loginViewModel)
        {
            LoginOutputViewModel result = null;

            if (!ModelState.IsValid) return GenerateResponse(ModelState);

            string accessToken = await _authenticationService.LoginUserAsync(loginViewModel.Email, loginViewModel.Password);

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                User user = await _userRepository.GetByEmailAsync(loginViewModel.Email);
                if (user == null)
                {
                    Notify("Revise suas credenciais");
                }
                else
                {
                    result = new()
                    {
                        Id = user.UserId,
                        Email = user.Email,
                        Name = user.Name,
                        AccessToken = accessToken
                    };
                }
            }

            return GenerateResponse(result);
        }
    }
}
