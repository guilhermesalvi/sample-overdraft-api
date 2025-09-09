namespace CustomerEnrollment.Features.BankAccounts.Domain;

public record BankAccount
{
    public required Guid CustomerId { get; init; }
    public required CustomerType CustomerType { get; init; }
    public required bool IsBankAccountActive { get; init; }

    public Guid Id { get; private init; } = Guid.CreateVersion7();
    public DateTimeOffset CreatedAt { get; private init; } = DateTimeOffset.UtcNow;
}
