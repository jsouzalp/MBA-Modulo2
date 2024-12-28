using FinPlanner360.Busines.Interfaces.Services;
using FinPlanner360.Busines.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.IdentityModel.Tokens.Jwt;

namespace FinPlanner360.Api.Controllers;

[ApiController]
public class MainController : ControllerBase
{
    internal Guid UserId
    {
        get
        {
            HttpContext.Items.TryGetValue(JwtRegisteredClaimNames.Sub, out var userIdValue);
            return Guid.Parse(userIdValue?.ToString() ?? Guid.Empty.ToString());
        }
    }

    internal string UserEmail
    {
        get
        {
            HttpContext.Items.TryGetValue(JwtRegisteredClaimNames.Email, out var emailValue);
            return emailValue?.ToString() ?? string.Empty;
        }
    }

    private readonly INotificationService _notificationService;

    protected MainController(INotificationService notificationService)
    {
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

    protected ActionResult GenerateResponse(object result = null, int statusCode = StatusCodes.Status200OK)
    {
        if (!_notificationService.HasNotification())
        {
            return new JsonResult(result)
            {
                StatusCode = statusCode,
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