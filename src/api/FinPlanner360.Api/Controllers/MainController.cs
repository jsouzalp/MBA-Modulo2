using FinPlanner360.Business.Interfaces.Services;
using FinPlanner360.Business.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace FinPlanner360.Api.Controllers;

[ApiController]
public class MainController : ControllerBase
{
    private readonly IAppIdentityUser _appIdentityUser;
    private readonly INotificationService _notificationService;
    
    public Guid UserId => _appIdentityUser.GetUserId();
    public bool IsAuthenticated => _appIdentityUser.IsAuthenticated();
    public string UserEmail => _appIdentityUser.GetUserEmail();

    protected MainController(IAppIdentityUser appIdentityUser, INotificationService notificationService)
    {
        _appIdentityUser = appIdentityUser;
        _notificationService = notificationService;
    }

    protected ActionResult GenerateResponse(object result = null, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        if (!_notificationService.HasNotification() || !_notificationService.HasError())
        {
            return new JsonResult(result)
            {
                StatusCode = Convert.ToInt32(statusCode),
                Value = new
                {
                    success = true,
                    result,
                    notifications = _notificationService.GetNotifications().ToList()//.Select(n => n.Message)
                }
            };
        }
        else
        {
            if ((int)statusCode >= 200 && (int)statusCode <= 299)
            {
                return BadRequest(new
                {
                    success = false,
                    errors = _notificationService.GetNotifications().Select(n => n.Message)
                });
            }

            return new JsonResult(new
            {
                success = false,
                errors = _notificationService.GetNotifications().Select(n => n.Message)
            })
            {
                StatusCode = Convert.ToInt32(statusCode)
            };
        }
    }

    protected ActionResult GenerateResponse(ModelStateDictionary modelState)
    {
        if (!modelState.IsValid) NotifyInvalidModel(modelState);
        return GenerateResponse();
    }

    protected void Notify(string message)
    {
        _notificationService.Handle(new Notification(message));
    }

    protected void NotifyInvalidModel(ModelStateDictionary modelState)
    {
        var errors = modelState.Values.SelectMany(e => e.Errors);

        foreach (var error in errors)
        {
            Notify(error?.Exception?.Message ?? error?.ErrorMessage);
        }
    }

    protected bool ValidateFileType(string fileType)
    {
        if (string.Equals(fileType, "pdf", StringComparison.OrdinalIgnoreCase) || string.Equals(fileType, "xlsx", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        Notify("Tipo de arquivo inválido. Use 'pdf' ou 'xlsx'.");
        return false;
    }

}