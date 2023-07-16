using MicroLine.Services.Booking.Domain.Common.Exceptions;
using MicroLine.Services.Booking.WebApi.Common.Exceptions;

namespace MicroLine.Services.Booking.WebApi.Common.Middleware;

internal class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException exception)
        {
            await HandleDomainExceptionAsync(context, exception);
        }
        catch (ApplicationExceptionBase exception)
        {
            await HandleApplicationExceptionAsync(context, exception);
        }
        catch (Exception exception)
        {
            await HandleUnexpectedExceptionsAsync(context, exception);
        }
    }

    private async Task HandleDomainExceptionAsync(HttpContext context, DomainException exception)
    {
        await ReturnProblemResponseAsync(
            exception.Code,
            exception.Message,
            StatusCodes.Status400BadRequest,
            context);
    }


    private async Task HandleApplicationExceptionAsync(HttpContext context, ApplicationExceptionBase exception)
    {
        var statusCode = exception is NotFoundException ?
            StatusCodes.Status404NotFound : StatusCodes.Status400BadRequest;

        await ReturnProblemResponseAsync(
            exception.Code,
            exception.Message,
            statusCode,
            context);
    }


    private async Task HandleUnexpectedExceptionsAsync(HttpContext context, Exception exception)
    {
        await ReturnProblemResponseAsync(
            null,
            exception.Message,
            StatusCodes.Status500InternalServerError,
            context);

        _logger.LogError(exception, exception.Message);
    }


    private async Task ReturnProblemResponseAsync(
        string? exceptionCode,
        string exceptionMessage,
        int statusCode,
        HttpContext context)
    {
        Dictionary<string, object?> extensions = new();

        if (exceptionCode is not null)
            extensions.Add("exceptionCode", exceptionCode);


        await Results.Problem(
                detail: exceptionMessage,
                statusCode: statusCode,
                instance: context.Request.Path,
                extensions: extensions
            )
            .ExecuteAsync(context);
    }
}