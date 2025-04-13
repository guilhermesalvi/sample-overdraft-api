using Microsoft.AspNetCore.Mvc;
using Overdraft.Api.Data;

namespace Overdraft.Api.Features.Contracting.DeleteContract;

public static class DeleteContractEndpoint
{
    public static void MapDeleteContractEndpoint(this RouteGroupBuilder builder)
    {
        builder
            .MapDelete("", DeleteContract)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("DeleteContract")
            .WithSummary("Deletes a given contract.");
    }

    private static async Task<IResult> DeleteContract(
        [FromQuery] Guid id,
        [FromServices] ContractRepository repository,
        CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(id, cancellationToken);
        return Results.NoContent();
    }
}
