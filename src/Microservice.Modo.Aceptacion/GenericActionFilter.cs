using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Microservice.Modo.Aceptacion;

public class GenericActionFilter : ActionFilterAttribute
{
    private readonly ILogger<GenericActionFilter> _logger;

    public GenericActionFilter(ILogger<GenericActionFilter> logger)
    {
        _logger = logger;
    }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        Log("OnActionExecuting ", filterContext.RouteData);
    }

    public override void OnActionExecuted(ActionExecutedContext filterContext)
    {
        if (filterContext.Exception != null)
        {
            var ex = filterContext.Exception;
            _logger.LogError(ex.Message, ex);
        }
        else
        {
            Log("OnActionExecuted", filterContext.RouteData);
        }
    }

    public override void OnResultExecuting(ResultExecutingContext filterContext)
    {
        Log("OnResultExecuting", filterContext.RouteData);
    }

    public override void OnResultExecuted(ResultExecutedContext filterContext)
    {
        Log("OnResultExecuted", filterContext.RouteData);
    }


    private void Log(string methodName, RouteData routeData)
    {
        var controllerName = routeData.Values["controller"];
        var actionName = routeData.Values["action"];
        var message = $"{methodName} controller:{controllerName} action:{actionName}";
        _logger.LogDebug(message, "Action Filter Log");
    }
}