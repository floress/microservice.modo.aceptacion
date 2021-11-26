using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Modo.Clients;
using WyD.Mess.WebApi.Exceptions;

namespace Microservice.Modo.Aceptacion
{
    public class ExceptionToResponseMapper : IExceptionToResponseMapper
    {
        public ExceptionResponse Map(Exception exception)
        {
            switch (exception)
            {
                case ApiException apiException:
                    var apiProblemDetails = new ProblemDetails
                    {
                        Status = apiException.ErrorCode,
                        Title = apiException.Message,
                        Detail = apiException.StackTrace
                    };
                    return new ExceptionResponse(apiProblemDetails, (HttpStatusCode)apiException.ErrorCode);
                default:
                    {
                        var problemDetails = new ProblemDetails
                        {
                            Status = (int)HttpStatusCode.InternalServerError,
                            Title = $"{exception.GetType().Name}: {exception.Message}",
                            Detail = exception.StackTrace
                        };

                        return new ExceptionResponse(problemDetails, HttpStatusCode.InternalServerError);
                    }

            }
        }
    }
}
