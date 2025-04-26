namespace Domain.Models;

public record ContractAgreement
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public bool IsContractAgreementActive { get; init; }

    public Guid AccountId { get; init; }
    public Guid ContractId { get; init; }
    public DateTimeOffset SignatureDate { get; init; }
}
