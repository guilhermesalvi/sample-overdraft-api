using Microsoft.AspNetCore.Mvc;
using Overdraft.Api.Data;
using Overdraft.Api.Models;

namespace Overdraft.Api.Features.Accounting.GetDailyLimit;

public static class GetDailyLimitEndpoint
{
    public static void MapGetDailyLimitEndpoint(this RouteGroupBuilder builder)
    {
        builder
            .MapGet("daily-limit", GetDailyLimit)
            .Produces<List<DailyLimit>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("GetDailyLimit")
            .WithSummary("Get all daily limits for a given account");
    }

    private static async Task<IResult> GetDailyLimit(
        [FromQuery] Guid accountId,
        [FromServices] DailyLimitRepository repository,
        CancellationToken cancellationToken)
    {
        var dailyLimits = await repository.GetByAccountIdAsync(accountId, cancellationToken);
        return Results.Ok(dailyLimits);
    }
}
