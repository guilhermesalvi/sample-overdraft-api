namespace Overdraft.Domain.Accounts;

public record Account
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public Guid ContractId { get; init; }

    public decimal PrincipalAmount { get; init; }
    public decimal ApprovedOverdraftLimit { get; init; }
    public decimal SelfDeclaredLimit { get; init; }
    public int UsedDaysInCurrentCycle { get; init; }
    public decimal UsedOverLimit { get; init; }

    public bool IsAccountActive { get; init; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}
