using GSalvi.Toolkit.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Overdraft.Application.UseCases.CreateAccount;
using Overdraft.Application.UseCases.DeleteAccount;
using Overdraft.Application.UseCases.GetAccount;
using Overdraft.Domain.Accounts;

namespace Overdraft.Api.Endpoints.V1;

public static class AccountEndpoints
{
    public static void MapAccountEndpoints(this RouteGroupBuilder builder)
    {
        var group = builder
            .MapGroup("/account");

        group
            .MapGet("", async (
                [FromServices] IMediator mediator,
                [FromQuery] Guid? id,
                CancellationToken cancellationToken) =>
            {
                var input = new GetAccountInput(id);
                var result = await mediator.Send(input, cancellationToken);

                return Results.Ok(result);
            })
            .Produces<IEnumerable<Account>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("GetAccount")
            .WithTags("account")
            .WithSummary("Get all active accounts or a specific account.");

        group
            .MapPost("", async (
                [FromServices] IMediator mediator,
                [FromServices] LocalizedNotificationManager notificationManager,
                [FromBody] CreateAccountInput request,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(request, cancellationToken);

                return notificationManager.HasNotifications
                    ? notificationManager.Notifications.ToValidationProblem()
                    : Results.Created($"/account/{result.Id}", result);
            })
            .Accepts<CreateAccountInput>("application/json")
            .Produces<Account>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("CreateAccount")
            .WithTags("account")
            .WithSummary("Creates a new account.");

        group
            .MapDelete("", async (
                [FromServices] IMediator mediator,
                [FromQuery] Guid id,
                CancellationToken cancellationToken) =>
            {
                var input = new DeleteAccountInput(id);
                await mediator.Send(input, cancellationToken);

                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("DeleteAccount")
            .WithTags("account")
            .WithSummary("Deletes a given account.");
    }
}
