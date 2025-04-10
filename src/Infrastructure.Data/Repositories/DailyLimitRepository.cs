using Microsoft.EntityFrameworkCore;
using Overdraft.Domain.Accounts;
using Overdraft.Infrastructure.Data.Context;

namespace Overdraft.Infrastructure.Data.Repositories;

public class DailyLimitRepository(
    ApplicationDbContext context) : IDailyLimitRepository
{
    public async Task<List<DailyLimit>> GetByReferenceDateAsync(
        Guid? accountId,
        DateTimeOffset? startDate,
        DateTimeOffset? endDate,
        CancellationToken cancellationToken)
    {
        var dailyLimits = await context.DailyLimits
            .Where(x => accountId == null || x.AccountId == accountId.Value)
            .Where(x => startDate == null || x.ReferenceDate >= startDate.Value.Date)
            .Where(x => endDate == null || x.ReferenceDate <= endDate.Value.Date.AddDays(1).AddTicks(-1))
            .OrderBy(x => x.ReferenceDate)
            .ToListAsync(cancellationToken);

        return dailyLimits;
    }

    public Task<int> GetUsedDaysAsync(
        Guid accountId, DateTimeOffset referenceDate, CancellationToken cancellationToken)
    {
        return context.DailyLimits
            .Where(x => x.AccountId == accountId)
            .Where(x => x.ReferenceDate.Date == referenceDate.Date)
            .Where(x => x.UsedLimit > 0)
            .CountAsync(cancellationToken: cancellationToken);
    }

    public Task CreateAsync(DailyLimit dailyLimit, CancellationToken cancellationToken)
    {
        context.DailyLimits.Add(dailyLimit);
        return context.SaveChangesAsync(cancellationToken);
    }

    public Task DeleteAsync(DailyLimit dailyLimit, CancellationToken cancellationToken)
    {
        context.DailyLimits.Remove(dailyLimit);
        return context.SaveChangesAsync(cancellationToken);
    }
}
