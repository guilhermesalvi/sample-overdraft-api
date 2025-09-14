using System.Diagnostics;
using CustomerEnrollment.CrossCutting.Diagnostics;
using FluentValidation;
using ServiceDefaults;

namespace CustomerEnrollment.CrossCutting.Filters;

public sealed class ValidationFilter<TRequest>(
    ActivitySource activitySource,
    IValidator<TRequest>? validator) : IEndpointFilter where TRequest : class
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext ctx,
        EndpointFilterDelegate next)
    {
        using var act = activitySource.StartBusinessActivity(SpanNames.RequestValidate);

        act?.AddEvent(new ActivityEvent(EventNames.ValidationStart));
        act?.SetTag("validation.request_type", typeof(TRequest).Name);

        try
        {
            var body = ctx.Arguments.OfType<TRequest>().FirstOrDefault();
            if (body is null)
            {
                act?.SetTag("validation.skipped", true);
                act?.AddEvent(new ActivityEvent(EventNames.ValidationEnd));
                act?.SetStatus(ActivityStatusCode.Ok);
                return await next(ctx);
            }

            if (validator is null)
            {
                act?.SetTag("validation.registered", false);
                act?.AddEvent(new ActivityEvent(EventNames.ValidationEnd));
                act?.SetStatus(ActivityStatusCode.Ok);
                return await next(ctx);
            }

            act?.SetTag("validation.registered", true);

            var result = await validator.ValidateAsync(body, ctx.HttpContext.RequestAborted);

            act?.SetTag("validation.errors", result.Errors.Count);
            if (!result.IsValid)
            {
                act?.SetStatus(ActivityStatusCode.Ok);
                act?.AddEvent(new ActivityEvent(EventNames.ValidationEnd));
                return Results.ValidationProblem(result.ToDictionary());
            }

            act?.AddEvent(new ActivityEvent(EventNames.ValidationEnd));
            act?.SetStatus(ActivityStatusCode.Ok);
            return await next(ctx);
        }
        catch (Exception ex)
        {
            act?.AddException(ex);
            act?.SetStatus(ActivityStatusCode.Error, ex.Message);
            act?.AddEvent(new ActivityEvent(EventNames.Error));
            throw;
        }
    }
}
