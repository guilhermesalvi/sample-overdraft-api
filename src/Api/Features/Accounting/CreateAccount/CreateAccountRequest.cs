using Overdraft.Api.Models;

namespace Overdraft.Api.Features.Accounting.CreateAccount;

public record CreateAccountRequest(
    Guid ContractId,
    decimal OverdraftLimit,
    decimal SelectedLimit)
{
    public Account ToContract()
    {
        return new Account
        {
            ContractId = ContractId,
            OverdraftLimit = OverdraftLimit,
            SelectedLimit = SelectedLimit,
            IsAccountActive = true
        };
    }
}
