using FluentValidation;

namespace CustomerEnrollment.CrossCutting.Filters;

public sealed class ValidationFilter<TRequest> : IEndpointFilter where TRequest : class
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext ctx,
        EndpointFilterDelegate next)
    {
        var body = ctx.Arguments.OfType<TRequest>().FirstOrDefault();
        if (body is null) return await next(ctx);

        var validator = ctx.HttpContext.RequestServices.GetService<IValidator<TRequest>>();
        if (validator is null) return await next(ctx);

        var result = await validator.ValidateAsync(body, ctx.HttpContext.RequestAborted);
        if (!result.IsValid) return Results.ValidationProblem(result.ToDictionary());

        return await next(ctx);
    }
}
