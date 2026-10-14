
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;

namespace Shop.Exceptions;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, message) = exception switch
        {
            ValidationException =>
                (StatusCodes.Status400BadRequest,
                    "Validation failed."),

            ArgumentException =>
                (StatusCodes.Status400BadRequest,
                    "Invalid request."),

            KeyNotFoundException =>
                (StatusCodes.Status404NotFound,
                    "Resource not found."),

            UnauthorizedAccessException =>
                (StatusCodes.Status403Forbidden,
                    "Access denied."),

            BadHttpRequestException badRequest =>
                (badRequest.StatusCode,
                    "Invalid request."),

            _ =>
                (StatusCodes.Status500InternalServerError,
                    exception.Message)
        };

        if (statusCode >= 500)
        {
            _logger.LogError(
                exception,
                "Unhandled exception occurred.");
        }
        else
        {
            _logger.LogWarning(
                exception,
                "Request failed with status code {StatusCode}.",
                statusCode);
        }

        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsJsonAsync(
            new
            {
                success = false,
                message,
                statusCode
            },
            cancellationToken);

        return true;
    }
}
