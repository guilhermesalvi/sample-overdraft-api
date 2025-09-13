namespace CustomerEnrollment.Models;

public record OverdraftAccount(Guid CustomerId, CustomerType CustomerType, bool IsBankAccountActive)
{
    public Guid Id { get; private init; } = Guid.CreateVersion7();
    public DateTimeOffset CreatedAt { get; private init; } = DateTimeOffset.UtcNow;
}
