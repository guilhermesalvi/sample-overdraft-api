using System.Linq.Expressions;

namespace Overdraft.Domain.Accounts;

public interface IAccountRepository
{
    Task<List<Account>> GetAsync(
        Expression<Func<Account, bool>> predicate, CancellationToken cancellationToken);

    Task<Account?> GetBydIdAsync(Guid id, CancellationToken cancellationToken);
    Task CreateAsync(Account account, CancellationToken cancellationToken);
    Task UpdateAsync(Account account, CancellationToken cancellationToken);
}
