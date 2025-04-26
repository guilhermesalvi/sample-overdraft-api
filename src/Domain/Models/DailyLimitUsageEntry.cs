namespace Domain.Models;

public record DailyLimitUsageEntry
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public Guid AccountId { get; init; }
    public Guid ContractId { get; init; }
    public DateOnly ReferenceDate { get; init; }

    public decimal PrincipalAmount { get; init; }
    public decimal ApprovedOverdraftLimit { get; init; }
    public decimal UsedOverLimit { get; init; }
}
