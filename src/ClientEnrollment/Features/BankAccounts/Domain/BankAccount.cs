namespace ClientEnrollment.Features.BankAccounts.Domain;

public record BankAccount(
    Guid ClientId,
    ClientType ClientType,
    bool IsBankAccountActive)
{
    public Guid Id { get; private init; } = Guid.CreateVersion7();
    public DateTimeOffset CreatedAt { get; private init; } = DateTimeOffset.UtcNow;
}
