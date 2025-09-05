using FluentValidation;

namespace ClientEnrollment.BankAccount.Endpoints.CreateBankAccount;

public class CreateBankAccountRequestValidator : AbstractValidator<CreateBankAccountRequest>
{
    public CreateBankAccountRequestValidator(BankAccountRepository repo)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("CustomerId must be provided.")
            .Must(v => Guid.TryParse(v, out _)).WithMessage("CustomerId must be a valid GUID.")
            .CustomAsync(async (customerId, ctx, ct) =>
            {
                var result = await repo.CheckCustomerIdExistsAsync(Guid.Parse(customerId), ct);

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
            .NotEmpty().WithMessage("CustomerType must be provided.")
            .Must(v => Enum.TryParse<CustomerType>(v, out _))
            .WithMessage("CustomerType must be a valid value (e.g., 'individual', 'business').");
    }
}
