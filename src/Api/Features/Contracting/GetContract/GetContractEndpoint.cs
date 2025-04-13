using Microsoft.AspNetCore.Mvc;
using Overdraft.Api.Data;
using Overdraft.Api.Models;

namespace Overdraft.Api.Features.Contracting.GetContract;

public static class GetContractEndpoint
{
    public static void MapGetContractEndpoint(this RouteGroupBuilder builder)
    {
        builder
            .MapGet("", GetContract)
            .Produces<List<Contract>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("GetContract")
            .WithSummary("Get all active contracts");
    }

    private static async Task<IResult> GetContract(
        [FromQuery] Guid? id,
        [FromServices] ContractRepository repository,
        CancellationToken cancellationToken)
    {
        var contracts = await repository.GetAsync(id, cancellationToken);
        return Results.Ok(contracts);
    }
}
