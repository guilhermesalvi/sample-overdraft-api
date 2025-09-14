using System.Diagnostics;
using CustomerEnrollment.CrossCutting.Database.Context;
using CustomerEnrollment.CrossCutting.Diagnostics;
using CustomerEnrollment.OverdraftAccounts.Aggregates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceDefaults;

namespace CustomerEnrollment.OverdraftAccounts.Endpoints.GetOverdraftAccounts;

public static class GetOverdraftAccountsEndpoint
{
    public static void MapGetOverdraftAccountsEndpoint(this RouteGroupBuilder builder)
    {
        builder
            .MapGet("", GetOverdraftAccountsAsync)
            .Produces<IEnumerable<OverdraftAccount>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("get-overdraft-accounts")
            .WithSummary("Retrieves overdraft accounts by filter criteria");
    }

    private static async Task<IResult> GetOverdraftAccountsAsync(
        [FromServices] CustomerEnrollmentDbContext context,
        [FromServices] ILoggerFactory loggerFactory,
        [FromServices] ActivitySource activitySource,
        [FromQuery] Guid? accountId,
        [FromQuery] Guid? customerId,
        [FromQuery] int? offset,
        [FromQuery] int? limit,
        CancellationToken ct)
    {
        var logger = loggerFactory.CreateLogger(typeof(GetOverdraftAccountsEndpoint).FullName!);

        using var act = activitySource.StartBusinessActivity(SpanNames.OverdraftAccountsGet);
        act?.SetTag("feature", "get-overdraft-accounts");
        act?.AddEvent(new ActivityEvent(EventNames.HandlerStart));
        act?.SetTag("filter.account_id.present", accountId.HasValue);
        act?.SetTag("filter.customer_id.present", customerId.HasValue);
        act?.SetTag("paging.offset.present", offset.HasValue);
        act?.SetTag("paging.limit.present", limit.HasValue);

        try
        {
            var page = Math.Max(0, offset ?? 0);
            var size = Math.Clamp(limit ?? 100, 1, 500);

            var query = context.OverdraftAccounts.AsNoTracking().AsQueryable();

            if (accountId.HasValue) query = query.Where(x => x.Id == accountId.Value);
            if (customerId.HasValue) query = query.Where(x => x.CustomerId == customerId.Value);

            query = query.OrderBy(x => x.Id);

            act?.AddEvent(new ActivityEvent(EventNames.QueryStart));
            var accounts = await query
                .Skip(page)
                .Take(size)
                .ToListAsync(ct);
            act?.AddEvent(new ActivityEvent(EventNames.QueryEnd));

            act?.SetStatus(ActivityStatusCode.Ok);
            act?.AddEvent(new ActivityEvent(EventNames.HandlerEnd));

            return TypedResults.Ok(accounts);
        }
        catch (Exception ex)
        {
            act?.SetStatus(ActivityStatusCode.Error, ex.Message);
            act?.AddException(ex);
            act?.AddEvent(new ActivityEvent(EventNames.Error));

            logger.LogError(ex, "An error occurred while retrieving overdraft accounts.");
            return TypedResults.Problem("An error occurred while retrieving overdraft accounts.", statusCode: 500);
        }
    }
}
