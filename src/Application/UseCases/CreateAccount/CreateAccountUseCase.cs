using GSalvi.Toolkit.Notifications;
using MediatR;
using Overdraft.Domain.Accounts;

namespace Overdraft.Application.UseCases.CreateAccount;

public class CreateAccountUseCase(
    IAccountRepository accountRepository,
    IContractRepository contractRepository,
    LocalizedNotificationManager notificationManager) : IRequestHandler<CreateAccountInput, Account>
{
    public async Task<Account> Handle(
        CreateAccountInput request, CancellationToken cancellationToken)
    {
        var contract = await contractRepository.GetByIdAsync(request.ContractId, cancellationToken);

        if (contract is null)
        {
            notificationManager.AddNotification("ContractNotFound");
            return null!;
        }

        var account = request.ToDomain();
        account = account with { IsAccountActive = true };
        await accountRepository.CreateAsync(account, cancellationToken);

        return account;
    }
}
