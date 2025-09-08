using ClientEnrollment.Features.BankAccounts.Repositories;
using FluentValidation;

namespace ClientEnrollment.Features.BankAccounts.Endpoints.CreateBankAccount;

public class CreateBankAccountRequestValidator : AbstractValidator<CreateBankAccountRequest>
{
    public CreateBankAccountRequestValidator(BankAccountRepository repository)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.ClientId)
            .NotEmpty().WithMessage("ClientId must be provided.")
            .CustomAsync(async (clientId, ctx, ct) =>
            {
                var result = await repository.CheckCustomerIdExistsAsync(clientId, ct);

                if (result.IsFailure)
                {
                    ctx.AddFailure(nameof(CreateBankAccountRequest.ClientId), result.Error);
                    return;
                }

                if (result.Value)
                {
                    ctx.AddFailure(nameof(CreateBankAccountRequest.ClientId),
                        "CustomerId already has an associated bank account.");
                }
            });

        RuleFor(x => x.ClientType)
            .IsInEnum().WithMessage("ClientType must be a valid enum value.");
    }
}
