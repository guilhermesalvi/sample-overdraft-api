namespace CustomerEnrollment.OverdraftContracts;

public record OverdraftContract
{
    public Guid Id { get; private init; } = Guid.CreateVersion7();
    public DateTimeOffset CreatedAt { get; private init; } = DateTimeOffset.UtcNow;

    public required Guid OverdraftAccountId { get; init; }
    public required int GracePeriodDays { get; init; }
    public required decimal MonthlyInterestRate { get; init; }
    public required decimal MonthlyOverLimitInterestRate { get; init; }
    public required decimal OverLimitFixedFee { get; init; }
    public required decimal MonthlyLatePaymentInterestRate { get; init; }
    public required decimal LatePaymentPenaltyRate { get; init; }

    public bool IsOverdraftContractActive { get; private init; }
    public DateTimeOffset? SignatureDate { get; private init; }
    public DateTimeOffset? CanceledAt { get; private init; }
}
