using FinPlanner360.Business.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FinPlanner360.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[Controller]")]
public class TestsController : MainController
{

    public TestsController(IAppIdentityUser appIdentityUser, INotificationService notificationService) 
        : base(appIdentityUser, notificationService)
    {
    }

    [HttpGet("Hello")]
    [Authorize(Roles = "USER")]
    public async Task<string> GetAsync()
    {
        return $"Hi user id {UserId} with email {UserEmail}. Welcome";
    }

}
