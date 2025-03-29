using MediatR;
using Overdraft.Domain.Accounts;

namespace Overdraft.Application.UseCases.CreateAccount;

public record CreateAccountInput(
    Guid ContractId,
    decimal OverdraftLimit,
    decimal SelectedLimit) : IRequest<Account>
{
    public Account ToDomain()
    {
        return new Account
        {
            ContractId = ContractId,
            OverdraftLimit = OverdraftLimit,
            SelectedLimit = SelectedLimit,
        };
    }
}
