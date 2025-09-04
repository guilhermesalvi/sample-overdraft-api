namespace ClientEnrollment.BankAccount;

public record Account(
    Guid CustomerId,
    CustomerType CustomerType,
    bool IsAccountActive)
{
    public Guid Id { get; private init; } = Guid.CreateVersion7();
    public DateTimeOffset CreatedAt { get; private init; } = DateTimeOffset.UtcNow;
}
