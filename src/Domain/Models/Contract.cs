namespace Domain.Models;

public record Contract
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public bool IsContractActive { get; init; }

    public int GracePeriodDays { get; init; }
    public decimal MonthlyInterestRate { get; init; }
    public decimal MonthlyOverLimitInterestRate { get; init; }
    public decimal OverLimitFixedFee { get; init; }
    public decimal MonthlyLatePaymentInterestRate { get; init; }
    public decimal LatePaymentPenaltyRate { get; init; }

    public decimal DailyInterestRate => MonthlyInterestRate / 30;
    public decimal DailyOverLimitInterestRate => MonthlyOverLimitInterestRate / 30;
    public decimal DailyLatePaymentInterestRate => MonthlyLatePaymentInterestRate / 30;
}
