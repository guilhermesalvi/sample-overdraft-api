namespace Domain.Models;

public record ProductCondition
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public Guid ContractId { get; init; }

    public decimal MinAssetsHeld { get; init; }
    public decimal MaxAssetsHeld { get; init; }
}
