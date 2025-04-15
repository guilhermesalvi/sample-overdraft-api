namespace Overdraft.Domain.Accounts;

public record Contract
{
    public Guid Id { get; init; } = Guid.CreateVersion7();

    public int GracePeriodDays { get; init; }
    public decimal MonthlyInterestRate { get; init; }
    public decimal MonthlyOverLimitInterestRate { get; init; }
    public decimal MonthlyLatePaymentInterestRate { get; init; }
    public decimal LatePaymentPenaltyRate { get; init; }

    public decimal DailyInterestRate => MonthlyInterestRate / 30;
    public decimal DailyOverLimitInterestRate => MonthlyOverLimitInterestRate / 30;
    public decimal DailyLatePaymentInterestRate => MonthlyLatePaymentInterestRate / 30;

    public bool IsContractActive { get; init; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}
