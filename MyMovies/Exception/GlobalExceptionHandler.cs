using Microsoft.AspNetCore.Diagnostics;
using MyMovies.Data.ViewModels;

namespace MyMovies.Exception;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        System.Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        var statusCode = exception is NotFoundException ? 404 : 500;

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";

        var errorResponse = new ErrorResponseVM
        {
            StatusCode = statusCode,
            Message = exception.Message,
            Timestamp = DateTime.UtcNow
        };

        await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

        return true;
    }
}