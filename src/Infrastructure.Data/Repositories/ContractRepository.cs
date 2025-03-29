using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Overdraft.Domain.Accounts;
using Overdraft.Infrastructure.Data.Context;

namespace Overdraft.Infrastructure.Data.Repositories;

public class ContractRepository(
    ApplicationDbContext context) : IContractRepository
{
    public async Task<IEnumerable<Contract>> GetAsync(
        Expression<Func<Contract, bool>> predicate, CancellationToken cancellationToken)
    {
        var contracts = await context.Contracts
            .Where(predicate)
            .ToListAsync(cancellationToken);

        return contracts;
    }

    public Task<Contract?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return context.Contracts
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public Task CreateAsync(Contract contract, CancellationToken cancellationToken)
    {
        context.Contracts.Add(contract);
        return context.SaveChangesAsync(cancellationToken);
    }

    public Task UpdateAsync(Contract contract, CancellationToken cancellationToken)
    {
        context.Contracts.Update(contract);
        return context.SaveChangesAsync(cancellationToken);
    }
}
