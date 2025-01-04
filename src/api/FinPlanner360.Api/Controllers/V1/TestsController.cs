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
    #region Mensagens para o Swagger
    private const string ObsoleteMessageDescription = "Este endpoint está obsoleto e será removido em versões futuras. Use '/api/v1/new-endpoint' em seu lugar.";
    private const string ObsoleteMessageSummary = "Este endpoint está obsoleto.";
    #endregion

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


    [HttpGet("Hello2")]
    [Authorize(Roles = "USER")]
    [Obsolete(ObsoleteMessageDescription)]
    [SwaggerOperation(Summary = ObsoleteMessageSummary, Description = ObsoleteMessageDescription)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<string>> GetID()
    {
        var responseMessage = $"Hi user id {UserId} with email {UserEmail}. Welcome 2";
        return Ok(responseMessage);
    }
}
