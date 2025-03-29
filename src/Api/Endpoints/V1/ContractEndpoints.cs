using GSalvi.Toolkit.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Overdraft.Application.UseCases.CreateContract;
using Overdraft.Application.UseCases.DeleteContract;
using Overdraft.Application.UseCases.GetContract;
using Overdraft.Domain.Accounts;

namespace Overdraft.Api.Endpoints.V1;

public static class ContractEndpoints
{
    public static void MapContractEndpoints(this RouteGroupBuilder builder)
    {
        var group = builder
            .MapGroup("/contract");

        group
            .MapGet("", async (
                [FromServices] IMediator mediator,
                [FromQuery] Guid? id,
                CancellationToken cancellationToken) =>
            {
                var input = new GetContractInput(id);
                var result = await mediator.Send(input, cancellationToken);

                return Results.Ok(result);
            })
            .Produces<IEnumerable<Contract>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("GetContracts")
            .WithTags("contract")
            .WithSummary("Get all active contracts or a specific contract.");

        group
            .MapPost("", async (
                [FromServices] IMediator mediator,
                [FromServices] LocalizedNotificationManager notificationManager,
                [FromBody] CreateContractInput request,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(request, cancellationToken);

                return notificationManager.HasNotifications
                    ? notificationManager.Notifications.ToValidationProblem()
                    : Results.Created($"/contract/{result.Id}", result);
            })
            .Accepts<CreateContractInput>("application/json")
            .Produces<Contract>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("CreateContract")
            .WithTags("contract")
            .WithSummary("Creates a new contract.");

        group
            .MapDelete("", async (
                [FromServices] IMediator mediator,
                [FromQuery] Guid id,
                CancellationToken cancellationToken) =>
            {
                var input = new DeleteContractInput(id);
                await mediator.Send(input, cancellationToken);

                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("DeleteContract")
            .WithTags("contract")
            .WithSummary("Deletes a given contract.");
    }
}
