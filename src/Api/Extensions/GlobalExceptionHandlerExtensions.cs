using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Overdraft.Api.Extensions;

public static class GlobalExceptionHandlerExtensions
{
    public static void AddGlobalExceptionHandler(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
    }
}

internal sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    ProblemDetailsFactory problemDetailsFactory) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An unhandled exception occurred while processing the request");

        var problemDetails = problemDetailsFactory.CreateProblemDetails(
            httpContext,
            statusCode: StatusCodes.Status500InternalServerError,
            title: "An error occurred while processing your request",
            detail: exception.Message);

        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/problem+json";

        await Results.Problem(problemDetails).ExecuteAsync(httpContext);

        return true;
    }
}
