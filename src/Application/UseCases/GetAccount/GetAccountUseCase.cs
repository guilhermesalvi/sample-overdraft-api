using MediatR;
using Overdraft.Domain.Accounts;

namespace Overdraft.Application.UseCases.GetAccount;

public class GetAccountUseCase(
    IAccountRepository repository) : IRequestHandler<GetAccountInput, IEnumerable<Account>>
{
    public async Task<IEnumerable<Account>> Handle(
        GetAccountInput request, CancellationToken cancellationToken)
    {
        var contracts = await repository.GetAsync(query =>
                (request.Id == null || query.Id == request.Id) &&
                query.IsAccountActive,
            cancellationToken);

        return contracts;
    }
}
