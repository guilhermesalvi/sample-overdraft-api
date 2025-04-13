using Microsoft.AspNetCore.Mvc;
using Overdraft.Api.Data;
using Overdraft.Api.Models;

namespace Overdraft.Api.Features.Contracting.CreateContract;

public static class CreateContractEndpoint
{
    public static void MapCreateContractEndpoint(this RouteGroupBuilder builder)
    {
        builder
            .MapPost("", CreateContract)
            .Accepts<CreateContractRequest>("application/json")
            .Produces<Contract>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("CreateContract")
            .WithSummary("Creates a new contract.");
    }

    private static async Task<IResult> CreateContract(
        [FromBody] CreateContractRequest request,
        [FromServices] ContractRepository repository,
        CancellationToken cancellationToken)
    {
        var contract = request.ToContract();
        await repository.CreateAsync(contract, cancellationToken);

        return Results.Created($"/contract/{contract.Id}", contract);
    }
}
