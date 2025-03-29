using MediatR;
using Overdraft.Domain.Accounts;

namespace Overdraft.Application.UseCases.DeleteContract;

public class DeleteContractUseCase(
    IContractRepository repository) : IRequestHandler<DeleteContractInput>
{
    public async Task Handle(DeleteContractInput request, CancellationToken cancellationToken)
    {
        var contract = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (contract != null)
        {
            contract = contract with { IsContractActive = false };
            await repository.UpdateAsync(contract, cancellationToken);
        }
    }
}
