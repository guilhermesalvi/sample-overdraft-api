using Microsoft.AspNetCore.Mvc;
using Overdraft.Api.Data;
using Overdraft.Api.Models;

namespace Overdraft.Api.Features.Accounting.GetAccount;

public static class GetAccountEndpoint
{
    public static void MapGetAccountEndpoint(this RouteGroupBuilder builder)
    {
        builder
            .MapGet("", GetAccount)
            .Produces<List<Account>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("GetAccount")
            .WithSummary("Get all active account");
    }

    private static async Task<IResult> GetAccount(
        [FromQuery] Guid? id,
        [FromServices] AccountRepository repository,
        CancellationToken cancellationToken)
    {
        var accounts = await repository.GetAsync(id, cancellationToken);
        return Results.Ok(accounts);
    }
}
