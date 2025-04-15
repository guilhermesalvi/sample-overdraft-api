namespace Overdraft.Domain.Accounts;

public record ContractAgreement
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public Guid AccountId { get; init; }
    public Guid ContractId { get; init; }
    public DateTimeOffset SignatureDate { get; set; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}
