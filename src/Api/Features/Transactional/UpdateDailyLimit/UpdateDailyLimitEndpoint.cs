using Microsoft.AspNetCore.Mvc;
using Overdraft.Api.Data;
using Overdraft.Api.SeedWork.Filters;

namespace Overdraft.Api.Features.Transactional.UpdateDailyLimit;

public static class UpdateDailyLimitEndpoint
{
    public static void MapUpdateDailyLimitEndpoint(this RouteGroupBuilder builder)
    {
        builder
            .MapPost("daily-limit", UpdateDailyLimit)
            .AddEndpointFilter(new ValidationFilter<UpdateDailyLimitRequest>())
            .Accepts<UpdateDailyLimitRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("UpdateDailyLimit")
            .WithSummary("Updates daily limits.");
    }

    private static async Task<IResult> UpdateDailyLimit(
        [FromBody] UpdateDailyLimitRequest request,
        [FromServices] DailyLimitRepository repository,
        CancellationToken cancellationToken)
    {
        foreach (var item in request.Items)
        {
            var dailyLimit = await repository.GetOneByReferenceDateAsync(item.AccountId, item.ReferenceDate, cancellationToken);

            if (dailyLimit is not null)
            {
                await repository.DeleteAsync(dailyLimit.Id, cancellationToken);
            }

            var newDailyLimit = item.ToDailyLimit();
            await repository.CreateAsync(newDailyLimit, cancellationToken);
        }

        return Results.NoContent();
    }
}
