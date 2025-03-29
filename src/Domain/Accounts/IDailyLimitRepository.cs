namespace Overdraft.Domain.Accounts;

public interface IDailyLimitRepository
{
    Task<IEnumerable<DailyLimit>> GetByReferenceDateAsync(
        Guid? accountId,
        DateTimeOffset? startDate,
        DateTimeOffset? endDate,
        CancellationToken cancellationToken);

    Task<int> GetUsedDaysAsync(
        Guid accountId, DateTimeOffset referenceDate, CancellationToken cancellationToken);

    Task CreateAsync(DailyLimit dailyLimit, CancellationToken cancellationToken);
    Task DeleteAsync(DailyLimit dailyLimit, CancellationToken cancellationToken);
}
