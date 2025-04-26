namespace Domain.Models;

public record MonthlyChargeSnapshot
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public Guid AccountId { get; init; }
    public Guid ContractId { get; init; }
    public DateOnly ReferenceDate { get; init; }

    public decimal ApprovedOverdraftLimit { get; init; }
    public int UsedDaysInCurrentCycle { get; init; }
    public int GracePeriodDays { get; init; }

    public decimal TotalRegularInterestDue { get; init; }
    public decimal TotalOverLimitInterestDue { get; init; }
    public decimal TotalOverLimitFixedFeeDue { get; init; }
    public decimal TotalLatePaymentInterestDue { get; init; }
    public decimal TotalLatePaymentPenaltyDue { get; init; }
    public decimal TotalCreditTaxDue { get; init; }
    public decimal TotalFixedCreditTaxDue { get; init; }

    public decimal TotalDue {get; init; }
    public decimal CapitalizedPrincipal { get; init; }
}
