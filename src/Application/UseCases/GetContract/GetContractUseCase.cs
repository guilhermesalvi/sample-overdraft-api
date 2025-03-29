using MediatR;
using Overdraft.Domain.Accounts;

namespace Overdraft.Application.UseCases.GetContract;

public class GetContractUseCase(
    IContractRepository repository) : IRequestHandler<GetContractInput, IEnumerable<Contract>>
{
    public async Task<IEnumerable<Contract>> Handle(
        GetContractInput request, CancellationToken cancellationToken)
    {
        var contracts = await repository.GetAsync(query =>
                (request.Id == null || query.Id == request.Id) &&
                query.IsContractActive,
            cancellationToken);

        return contracts;
    }
}
