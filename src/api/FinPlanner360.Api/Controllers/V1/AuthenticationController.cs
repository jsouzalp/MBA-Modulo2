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

        //[HttpPost]
        //[Route("register")]
        //public async Task<ActionResult> Register(RegisterUserViewModel registerUserViewModel)
        //{
        //    if (!ModelState.IsValid) return CustomResponse(ModelState);
        //    var user = new IdentityUser
        //    {
        //        UserName = registerUserViewModel.Email,
        //        Email = registerUserViewModel.Email,
        //        EmailConfirmed = true
        //    };
        //    var result = await _userManager.CreateAsync(user, registerUserViewModel.Password);
        //    if (result.Succeeded)
        //    {
        //        return CustomResponse(await GenerateJwt(registerUserViewModel.Email));
        //    }
        //    foreach (var error in result.Errors)
        //    {
        //        NotificarErro(error.Description);
        //    }
        //    return CustomResponse();
        //}

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginViewModel loginViewModel)
        {
            LoginOutputViewModel result = null;

            if (!ModelState.IsValid) return GenerateResponse(ModelState, StatusCodes.Status400BadRequest);

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
