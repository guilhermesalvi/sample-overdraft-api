namespace Overdraft.Domain.Accounts;

public record DailyLimit
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public Guid AccountId { get; init; }

    public decimal UsedLimit { get; init; }

    public DateTimeOffset ReferenceDate { get; init; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}
