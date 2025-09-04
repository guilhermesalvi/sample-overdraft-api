namespace ClientEnrollment.OverdraftContract;

public record Contract(
    Guid AccountId,
    DateTimeOffset SignatureDate,
    bool IsContractActive,
    int GracePeriodDays,
    decimal MonthlyInterestRate,
    decimal MonthlyOverLimitInterestRate,
    decimal OverLimitFixedFee,
    decimal MonthlyLatePaymentInterestRate,
    decimal LatePaymentPenaltyRate)
{
    public Guid Id { get; private init; } = Guid.CreateVersion7();
    public DateTimeOffset CreatedAt { get; private init; } = DateTimeOffset.UtcNow;
}
