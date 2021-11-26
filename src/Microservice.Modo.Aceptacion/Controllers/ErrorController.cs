using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Modo.Clients;

namespace Microservice.Modo.Aceptacion.Controllers;

[ApiController]
[Route("[controller]")]
public class ErrorController : ControllerBase
{
    [Route("/error")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public ActionResult Error([FromServices] IWebHostEnvironment webHostEnvironment)
    {
        var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        if (feature == null)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new ProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Title = "Feature undefined"
            });
        }

        var ex = feature.Error;

        var isDev = webHostEnvironment.IsDevelopment();

        switch (ex)
        {
            case ApiException apiException:
                var apiProblemDetails = new ProblemDetails
                {
                    Status = apiException.ErrorCode,
                    Instance = feature.Path,
                    Title = apiException.Message,
                    Detail = isDev ? apiException.StackTrace : null,
                };
                return StatusCode(apiException.ErrorCode, apiProblemDetails);
            default:
            {
                var problemDetails = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Instance = feature.Path,
                    Title = isDev ? $"{ex.GetType().Name}: {ex.Message}" : "An error occurred.",
                    Detail = isDev ? ex.StackTrace : null,
                };

                return StatusCode(problemDetails.Status.Value, problemDetails);
            }
        }
    }
}