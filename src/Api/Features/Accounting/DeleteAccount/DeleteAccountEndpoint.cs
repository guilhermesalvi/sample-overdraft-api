using Microsoft.AspNetCore.Mvc;
using Overdraft.Api.Data;

namespace Overdraft.Api.Features.Accounting.DeleteAccount;

public static class DeleteAccountEndpoint
{
    public static void MapDeleteAccountEndpoint(this RouteGroupBuilder builder)
    {
        builder
            .MapDelete("", DeleteAccount)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("DeleteAccount")
            .WithSummary("Deletes a given account.");
    }

    private static async Task<IResult> DeleteAccount(
        [FromQuery] Guid id,
        [FromServices] AccountRepository repository,
        CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(id, cancellationToken);
        return Results.NoContent();
    }
}
