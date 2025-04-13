using FluentValidation;
using Overdraft.Api.Data;

namespace Overdraft.Api.Features.Accounting.CreateAccount;

public class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
{
    private readonly ContractRepository _repository;

    public CreateAccountRequestValidator(ContractRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.ContractId)
            .NotEmpty().WithMessage("ContractId is required.");

        RuleFor(x => x.ContractId)
            .MustAsync(ContractExists).WithMessage("ContractId does not exist.")
            .When(x => x.ContractId != Guid.Empty);
    }

    private async Task<bool> ContractExists(Guid contractId, CancellationToken cancellationToken)
    {
        var contract = await _repository.GetAsync(contractId, cancellationToken);
        return contract.Count > 0;
    }
}
