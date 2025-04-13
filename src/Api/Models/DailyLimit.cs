namespace Overdraft.Api.Models;

public record DailyLimit
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public Guid AccountId { get; init; }

    public decimal UsedLimit { get; init; }

    public DateOnly ReferenceDate { get; init; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}
