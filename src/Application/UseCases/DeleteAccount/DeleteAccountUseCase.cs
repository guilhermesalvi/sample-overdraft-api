using MediatR;
using Overdraft.Domain.Accounts;

namespace Overdraft.Application.UseCases.DeleteAccount;

public class DeleteAccountUseCase(
    IAccountRepository repository) : IRequestHandler<DeleteAccountInput>
{
    public async Task Handle(DeleteAccountInput request, CancellationToken cancellationToken)
    {
        var account = await repository.GetBydIdAsync(request.Id, cancellationToken);

        if (account != null)
        {
            account = account with { IsAccountActive = false };
            await repository.UpdateAsync(account, cancellationToken);
        }
    }
}
