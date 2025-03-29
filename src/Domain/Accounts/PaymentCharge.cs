namespace Overdraft.Domain.Accounts;

public record PaymentCharge
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public Guid AccountId { get; init; }
    public Guid ContractId { get; init; }

    public int GracePeriodDays { get; init; }
    public int UsedDays { get; init; }

    public decimal InterestDue { get; init; }
    public decimal IofTaxDue { get; init; }
    public decimal OverLimitInterestDue { get; init; }
    public decimal LatePaymentInterestDue { get; init; }
    public decimal LatePaymentPenaltyDue { get; init; }
    public decimal TotalDue { get; init; }

    public DateTimeOffset ReferenceDate { get; init; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}
