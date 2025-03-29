using GSalvi.Toolkit.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Overdraft.Application.UseCases.GetDailyLimit;
using Overdraft.Application.UseCases.UpdateDailyLimit;
using Overdraft.Domain.Accounts;

namespace Overdraft.Api.Endpoints.V1;

public static class DailyLimitEndpoints
{
    public static void MapDailyLimitEndpoints(this RouteGroupBuilder builder)
    {
        var group = builder
            .MapGroup("/daily-limit");

        group
            .MapGet("", async (
                [FromServices] IMediator mediator,
                [FromQuery] Guid accountId,
                [FromQuery] DateTimeOffset startDate,
                [FromQuery] DateTimeOffset endDate,
                CancellationToken cancellationToken) =>
            {
                var input = new GetDailyLimitInput(accountId, startDate, endDate);
                var result = await mediator.Send(input, cancellationToken);

                return Results.Ok(result);
            })
            .Produces<IEnumerable<DailyLimit>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("GetDailyLimit")
            .WithTags("daily-limit")
            .WithSummary("Get all daily limits of an account.");

        group
            .MapPost("", async (
                [FromServices] IMediator mediator,
                [FromServices] LocalizedNotificationManager notificationManager,
                [FromBody] IEnumerable<UpdateDailyLimitInputItem> request,
                CancellationToken cancellationToken) =>
            {
                var input = new UpdateDailyLimitInput(request);
                await mediator.Send(input, cancellationToken);

                return notificationManager.HasNotifications
                    ? notificationManager.Notifications.ToValidationProblem()
                    : Results.NoContent();
            })
            .Accepts<IEnumerable<UpdateDailyLimitInput>>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("UpdateDailyLimit")
            .WithTags("daily-limit")
            .WithSummary("Updates the daily limit of an account.");
    }
}
