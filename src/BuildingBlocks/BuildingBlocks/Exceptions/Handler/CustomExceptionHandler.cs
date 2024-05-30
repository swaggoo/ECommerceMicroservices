using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuildingBlocks.Exceptions.Handler;

public class CustomExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        (string Details, string Title, int StatusCode) details = exception switch
        {
            InternalServerException e => (e.Message, exception.GetType().Name, StatusCodes.Status500InternalServerError),
            ValidationException e => (e.Message, exception.GetType().Name, StatusCodes.Status400BadRequest),
            BadRequestException e => (e.Message, exception.GetType().Name, StatusCodes.Status400BadRequest),
            NotFoundException e => (e.Message, exception.GetType().Name, StatusCodes.Status404NotFound),
            _ => (exception.Message, exception.GetType().Name, StatusCodes.Status500InternalServerError)
        };
        
        var problemDetails = new ProblemDetails
        {
            Title = details.Title,
            Detail = details.Details,
            Status = details.StatusCode,
            Instance = httpContext.Request.Path
        };
        
        problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);

        if (exception is ValidationException validationException)
        {
            problemDetails.Extensions.Add("errors", validationException.Errors);
        }
        
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
        return true;
    }
}