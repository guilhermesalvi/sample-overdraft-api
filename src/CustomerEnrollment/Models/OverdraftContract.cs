namespace CustomerEnrollment.Models;

public record OverdraftContract(
    Guid OverdraftAccountId,
    int GracePeriodDays,
    decimal MonthlyInterestRate,
    decimal MonthlyOverLimitInterestRate,
    decimal OverLimitFixedFee,
    decimal MonthlyLatePaymentInterestRate,
    decimal LatePaymentPenaltyRate)
{
    public Guid Id { get; private init; } = Guid.CreateVersion7();
    public DateTimeOffset CreatedAt { get; private init; } = DateTimeOffset.UtcNow;

    public bool IsOverdraftContractActive { get; private init; }
    public DateTimeOffset? SignatureDate { get; private init; }
    public DateTimeOffset? CanceledAt { get; private init; }
}
