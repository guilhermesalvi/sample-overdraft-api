using CustomerEnrollment.Features.BankAccounts.Repositories;
using FluentValidation;

namespace CustomerEnrollment.Features.BankAccounts.Endpoints.CreateBankAccount;

public class CreateBankAccountRequestValidator : AbstractValidator<CreateBankAccountRequest>
{
    public CreateBankAccountRequestValidator(BankAccountRepository repository)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("CustomerId must be provided.")
            .CustomAsync(async (customerId, ctx, ct) =>
            {
                var result = await repository.CheckCustomerIdExistsAsync(customerId, ct);

                if (result.IsFailure)
                {
                    ctx.AddFailure(nameof(CreateBankAccountRequest.CustomerId), result.Error);
                    return;
                }

                if (result.Value)
                {
                    ctx.AddFailure(nameof(CreateBankAccountRequest.CustomerId),
                        "CustomerId already has an associated bank account.");
                }
            });

        RuleFor(x => x.CustomerType)
            .IsInEnum().WithMessage("CustomerType must be a valid enum value.");
    }
}
