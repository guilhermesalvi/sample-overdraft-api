using ClientEnrollment.CrossCutting.Filters;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ClientEnrollment.BankAccount.Endpoints.CreateBankAccount;

public static class CreateBankAccountEndpoint
{
    public static void MapCreateBankAccountEndpoint(this RouteGroupBuilder builder)
    {
        builder
            .MapPost("", CreateBankAccountAsync)
            .AddEndpointFilter(new ValidationFilter<CreateBankAccountRequest>())
            .Accepts<CreateBankAccountRequest>("application/json")
            .Produces<Account>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("create-bank-account")
            .WithTags("create", "register")
            .WithSummary("Creates a new bank account for a given customer.");
    }

    private static async Task<IResult> CreateBankAccountAsync(
        [FromServices] IValidator<CreateBankAccountRequest> validator,
        [FromServices] BankAccountRepository repository,
        [FromBody] CreateBankAccountRequest request,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(request, ct);

        if (!validationResult.IsValid)
            return TypedResults.ValidationProblem(validationResult.ToDictionary());

        var account = request.ToInactiveAccount();
        var result = await repository.CreateAccountAsync(account, ct);

        return result.IsSuccess
            ? TypedResults.Created($"/accounts/{account.Id}", account)
            : TypedResults.Problem(result.Error, statusCode: 500);
    }
}
