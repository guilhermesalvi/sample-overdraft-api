using System.Diagnostics;
using CustomerEnrollment.CrossCutting.Database.Context;
using CustomerEnrollment.CrossCutting.Diagnostics;
using CustomerEnrollment.CrossCutting.Filters;
using CustomerEnrollment.OverdraftAccounts.Aggregates;
using Microsoft.AspNetCore.Mvc;
using ServiceDefaults;

namespace CustomerEnrollment.OverdraftAccounts.Endpoints.CreateOverdraftAccount;

public static class CreateOverdraftAccountEndpoint
{
    public static void MapCreateOverdraftAccountEndpoint(this RouteGroupBuilder builder)
    {
        builder
            .MapPost("", CreateOverdraftAccountAsync)
            .AddEndpointFilter<ValidationFilter<CreateOverdraftAccountRequest>>()
            .Accepts<CreateOverdraftAccountRequest>("application/json")
            .Produces<OverdraftAccount>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("create-overdraft-account")
            .WithSummary("Creates a new overdraft account for a given customer.");
    }

    private static async Task<IResult> CreateOverdraftAccountAsync(
        [FromServices] CustomerEnrollmentDbContext context,
        [FromServices] ILoggerFactory loggerFactory,
        [FromServices] ActivitySource activitySource,
        [FromBody] CreateOverdraftAccountRequest request,
        CancellationToken ct)
    {
        var logger = loggerFactory.CreateLogger(typeof(CreateOverdraftAccountEndpoint).FullName!);

        using var act = activitySource.StartBusinessActivity(SpanNames.OverdraftAccountsCreate);
        act?.SetTag("feature", "create-overdraft-account");
        act?.AddEvent(new ActivityEvent(EventNames.HandlerStart));

        try
        {
            var account = request.ToInactiveOverdraftAccount();

            act?.AddEvent(new ActivityEvent(EventNames.QueryStart));
            await context.AddAsync(account, ct);
            await context.SaveChangesAsync(ct);
            act?.AddEvent(new ActivityEvent(EventNames.QueryEnd));

            act?.SetStatus(ActivityStatusCode.Ok);
            act?.AddEvent(new ActivityEvent(EventNames.HandlerEnd));

            return TypedResults.Created($"/overdraft-accounts/{account.Id}", account);
        }
        catch (Exception ex)
        {
            act?.SetStatus(ActivityStatusCode.Error, ex.Message);
            act?.AddException(ex);
            act?.AddEvent(new ActivityEvent(EventNames.Error));

            logger.LogError(ex, "Error creating overdraft account for customer {CustomerId}", request.CustomerId);
            return TypedResults.Problem("Error creating overdraft account",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
