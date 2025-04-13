using FluentValidation;
using Overdraft.Api.Data;

namespace Overdraft.Api.Features.Accounting.UpdateDailyLimit;

public class UpdateDailyLimitRequestValidator : AbstractValidator<UpdateDailyLimitRequest>
{
    public UpdateDailyLimitRequestValidator(AccountRepository repository)
    {
        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Items cannot be empty.");
    }
}

public class UpdateDailyLimitItemValidator : AbstractValidator<UpdateDailyLimitItem>
{
    private readonly AccountRepository _repository;

    public UpdateDailyLimitItemValidator(AccountRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("AccountId cannot be empty.")
            .MustAsync(AccountExists).WithMessage("Account does not exist.");
    }

    private async Task<bool> AccountExists(Guid accountId, CancellationToken cancellationToken)
    {
        var accounts = await _repository.GetAsync(accountId, cancellationToken);
        return accounts.Count > 0;
    }
}
