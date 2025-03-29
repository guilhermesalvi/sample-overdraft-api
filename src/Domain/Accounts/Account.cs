namespace Overdraft.Domain.Accounts;

public record Account
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public Guid ContractId { get; init; }

    public decimal UsedLimit { get; init; }
    public decimal OverdraftLimit { get; init; }
    public decimal SelectedLimit { get; init; }
    public int UsedDays { get; init; }

    public decimal UsedOverLimit => UsedLimit >= OverdraftLimit ? UsedLimit - OverdraftLimit : 0;

    public bool IsAccountActive { get; init; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}
