using CustomerEnrollment.Features.BankAccounts.Domain;
using CustomerEnrollment.Features.BankAccounts.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CustomerEnrollment.Features.BankAccounts.Endpoints.GetBankAccounts;

public static class GetBankAccountsEndpoint
{
    public static void MapGetBankAccountsEndpoint(this RouteGroupBuilder builder)
    {
        builder
            .MapGet("", GetBankAccountAsync)
            .Produces<IEnumerable<BankAccount>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("get-bank-accounts")
            .WithTags("get")
            .WithSummary("Retrieves all bank accounts or a specific one by its parameters.");
    }

    private static async Task<IResult> GetBankAccountAsync(
        [FromServices] BankAccountRepository repository,
        [FromQuery] Guid? accountId,
        [FromQuery] Guid? customerId,
        [FromQuery] CustomerType? customerType,
        [FromQuery] bool? isBankAccountActive,
        CancellationToken ct,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 10)
    {
        var result = await repository.GetBankAccountsAsync(
            accountId,
            customerId,
            customerType,
            isBankAccountActive,
            offset,
            limit,
            ct);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.Problem(result.Error, statusCode: 500);
    }
}
