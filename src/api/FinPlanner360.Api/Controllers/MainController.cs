using FinPlanner360.Business.Interfaces.Services;
using FinPlanner360.Business.Models;
using FinPlanner360.Business.Services;
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

        //if (appUser.IsAuthenticated())
        //{
        //    UsuarioId = appUser.GetUserId();
        //    UsuarioAutenticado = true;
        //}
    }

    //protected bool OperacaoValida()
    //{
    //    return !_notificador.TemNotificacao();
    //}

    protected ActionResult GenerateResponse(object result = null, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        if (!_notificationService.HasNotification())
        {
            return new JsonResult(result)
            {
                StatusCode = Convert.ToInt32(statusCode),
                Value = new
                {
                    success = true,
                    result = result
                }
            };
        }

        return BadRequest(new
        {
            success = false,
            errors = _notificationService.GetNotifications().Select(n => n.Message)
        });
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
}