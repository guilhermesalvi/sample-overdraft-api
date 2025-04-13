using FluentValidation;
using Overdraft.Api.Data;

namespace Overdraft.Api.Features.Accounting.CalculatePaymentCharge;

public class CalculatePaymentChargeRequestValidator : AbstractValidator<CalculatePaymentChargeRequest>
{
    private readonly AccountRepository _repository;

    public CalculatePaymentChargeRequestValidator(AccountRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("AccountId is required.")
            .MustAsync(AccountExists).WithMessage("AccountId does not exist.");
    }

    private async Task<bool> AccountExists(Guid accountId, CancellationToken cancellationToken)
    {
        var accounts = await _repository.GetAsync(accountId, cancellationToken);
        return accounts.Count > 0;
    }
}
