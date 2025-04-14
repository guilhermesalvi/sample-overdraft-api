namespace Overdraft.Api.Models;

public record DailyLimitUsageEntry
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public Guid AccountId { get; init; }
    public Guid ContractId { get; init; }
    public DateOnly ReferenceDate { get; init; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;

    public decimal PrincipalAmount { get; init; }
    public decimal ApprovedOverdraftLimit { get; init; }
    public decimal SelfDeclaredLimit { get; init; }
    public int AccumulatedUsedDays { get; init; }
    public decimal UsedOverLimit { get; init; }

    public int GracePeriodDays { get; init; }
    public decimal InterestRateApplied { get; init; }
    public decimal CreditTaxRateApplied { get; init; }
    public decimal FixedCreditTaxRateApplied {get; init; }
    public decimal OverLimitRateApplied { get; init; }
    public decimal LatePaymentRateApplied { get; init; }
    public decimal PenaltyRateApplied { get; init; }

    public decimal InterestDue { get; init; }
    public decimal CreditTaxDue { get; init; }
    public decimal FixedCreditTaxDue { get; init; }
    public decimal OverLimitInterestDue { get; init; }
    public decimal LatePaymentInterestDue { get; init; }
    public decimal LatePaymentPenaltyDue { get; init; }
    public decimal TotalDue { get; init; }
}
