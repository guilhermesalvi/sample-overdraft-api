using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Api.Extensions;

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
    IServiceProvider serviceProvider) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An unhandled exception occurred while processing the request");

        using var scope = serviceProvider.CreateScope();
        var problemDetailsFactory = scope.ServiceProvider.GetRequiredService<ProblemDetailsFactory>();

        var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
        var message = env.IsDevelopment()
            ? exception.Message
            : "An error occurred while processing your request";

        var problemDetails = problemDetailsFactory.CreateProblemDetails(
            httpContext,
            statusCode: StatusCodes.Status500InternalServerError,
            title: "An error occurred while processing your request",
            detail: message);

        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/problem+json";

        await Results.Problem(problemDetails).ExecuteAsync(httpContext);

        return true;
    }
}
