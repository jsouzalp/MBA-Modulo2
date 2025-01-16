using FinPlanner360.Business.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinPlanner360.Api.Controllers.V1
{
    [Authorize(Roles = "USER")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class ReportController : MainController
    {
        protected ReportController(IAppIdentityUser appIdentityUser, 
            INotificationService notificationService) : base(appIdentityUser, notificationService)
        {
        }
    }
}
