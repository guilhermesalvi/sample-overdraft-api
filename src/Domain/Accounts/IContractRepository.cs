using System.Linq.Expressions;

namespace Overdraft.Domain.Accounts;

public interface IContractRepository
{
    Task<IEnumerable<Contract>> GetAsync(
        Expression<Func<Contract, bool>> predicate, CancellationToken cancellationToken);

    Task<Contract?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task CreateAsync(Contract contract, CancellationToken cancellationToken);
    Task UpdateAsync(Contract contract, CancellationToken cancellationToken);
}
