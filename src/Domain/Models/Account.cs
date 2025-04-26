namespace Domain.Models;

public record Account
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public PersonType PersonType { get; init; }
    public bool IsAccountActive { get; init; }

    public decimal CustomerAssetsHeld { get; init; }

    public decimal PrincipalAmount { get; init; }
    public decimal ApprovedOverdraftLimit { get; init; }
    public decimal SelfDeclaredLimit { get; init; }
    public int UsedDaysInCurrentCycle { get; init; }
    public decimal UsedOverLimit { get; init; }

    public decimal RegularInterestDueInCurrentCycle { get; init; }
    public decimal OverLimitInterestDueInCurrentCycle { get; init; }
    public decimal OverLimitFixedFeeDueInCurrentCycle { get; init; }
    public decimal LatePaymentInterestDueInCurrentCycle { get; init; }
    public decimal LatePaymentPenaltyDueInCurrentCycle { get; init; }
    public decimal CreditTaxDueInCurrentCycle { get; init; }
    public decimal FixedCreditTaxDueInCurrentCycle { get; init; }
}
