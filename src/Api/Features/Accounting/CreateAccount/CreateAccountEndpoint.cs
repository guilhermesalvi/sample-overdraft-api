using Microsoft.AspNetCore.Mvc;
using Overdraft.Api.Data;
using Overdraft.Api.Models;
using Overdraft.Api.SeedWork.Filters;

namespace Overdraft.Api.Features.Accounting.CreateAccount;

public static class CreateAccountEndpoint
{
    public static void MapCreateAccountEndpoint(this RouteGroupBuilder builder)
    {
        builder
            .MapPost("", CreateAccount)
            .AddEndpointFilter(new ValidationFilter<CreateAccountRequest>())
            .Accepts<CreateAccountRequest>("application/json")
            .Produces<Account>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("CreateAccount")
            .WithSummary("Creates a new account.");
    }

    private static async Task<IResult> CreateAccount(
        [FromBody] CreateAccountRequest request,
        [FromServices] AccountRepository repository,
        CancellationToken cancellationToken)
    {
        var account = request.ToContract();
        await repository.CreateAsync(account, cancellationToken);

        return Results.Created($"/account/{account.Id}", account);
    }
}
