using FinPlanner360.Api.Filters;
using FinPlanner360.Busines.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinPlanner360.Api.Controllers.V1;

[ServiceFilter(typeof(ValidateAccessTokenFilter))]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[Controller]")]
public class TestsController : MainController
{
    public TestsController(INotificationService notificationService) 
        : base(notificationService)
    {
    }

    [HttpGet("hello")]
    public async Task<ActionResult> HelloAsync()
    {
        return GenerateResponse($"{UserId} with email {UserEmail}, Welcome to this API. {nameof(HelloAsync)} OK");
    }
}
