using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace FinPlanner360.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private readonly IActionResultExecutor<ObjectResult> _executor;
    public ExceptionFilter(IActionResultExecutor<ObjectResult> executor)
    {
        _executor = executor;
    }

    public void OnException(ExceptionContext context)
    {
        context.ExceptionHandled = true;
        var outputResponse = new
        {
            success = false,
            message = "Ops, aconteceu um erro inesperado", 
            internalMessage = context?.Exception?.Message ?? context.Exception?.ToString()
        };

        var output = new ObjectResult(outputResponse)
        {
            StatusCode = StatusCodes.Status500InternalServerError,
            Value = outputResponse
        };

        _executor.ExecuteAsync(new ActionContext(context.HttpContext, context.RouteData, context.ActionDescriptor), output)
            .GetAwaiter()
            .GetResult();
    }
}