using CustomerEnrollment.CrossCutting.Filters;
using CustomerEnrollment.Features.BankAccounts.Domain;
using CustomerEnrollment.Features.BankAccounts.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CustomerEnrollment.Features.BankAccounts.Endpoints.CreateBankAccount;

public static class CreateBankAccountEndpoint
{
    public static void MapCreateBankAccountEndpoint(this RouteGroupBuilder builder)
    {
        builder
            .MapPost("", CreateBankAccountAsync)
            .AddEndpointFilter(new ValidationFilter<CreateBankAccountRequest>())
            .Accepts<CreateBankAccountRequest>("application/json")
            .Produces<BankAccount>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("create-bank-account")
            .WithTags("create")
            .WithSummary("Creates a new bank account for a given customer.");
    }

    private static async Task<IResult> CreateBankAccountAsync(
        [FromServices] BankAccountRepository repository,
        [FromBody] CreateBankAccountRequest request,
        CancellationToken ct)
    {
        var account = request.ToInactiveBankAccount();
        var result = await repository.CreateBankAccountAsync(account, ct);

        return result.IsSuccess
            ? TypedResults.Created($"/accounts/{account.Id}", account)
            : TypedResults.Problem(result.Error, statusCode: 500);
    }
}
