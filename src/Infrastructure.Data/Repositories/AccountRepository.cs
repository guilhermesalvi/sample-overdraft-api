using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Overdraft.Domain.Accounts;
using Overdraft.Infrastructure.Data.Context;

namespace Overdraft.Infrastructure.Data.Repositories;

public class AccountRepository(
    ApplicationDbContext context) : IAccountRepository
{
    public async Task<IEnumerable<Account>> GetAsync(
        Expression<Func<Account, bool>> predicate, CancellationToken cancellationToken)
    {
        var accounts = await context.Accounts
            .Where(predicate)
            .ToListAsync(cancellationToken);

        return accounts;
    }

    public Task<Account?> GetBydIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return context.Accounts
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public Task CreateAsync(Account account, CancellationToken cancellationToken)
    {
        context.Accounts.Add(account);
        return context.SaveChangesAsync(cancellationToken);
    }

    public Task UpdateAsync(Account account, CancellationToken cancellationToken)
    {
        context.Accounts.Update(account);
        return context.SaveChangesAsync(cancellationToken);
    }
}
