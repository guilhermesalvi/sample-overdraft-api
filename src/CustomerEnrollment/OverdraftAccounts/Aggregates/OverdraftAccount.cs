namespace CustomerEnrollment.OverdraftAccounts.Aggregates;

public record OverdraftAccount
{
    public Guid Id { get; private init; } = Guid.CreateVersion7();
    public DateTimeOffset CreatedAt { get; private init; } = DateTimeOffset.UtcNow;

    public required Guid CustomerId { get; init; }
    public required CustomerType CustomerType { get; init; }
    public required bool IsOverdraftAccountActive { get; init; }
}
