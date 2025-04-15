namespace Overdraft.Domain.Accounts;

public static class InterestCalculator
{
    public static decimal CalculatePrincipalInterest(
        decimal principal, int accumulatedUsedDays, Contract contract)
    {
        if (principal <= 0 || accumulatedUsedDays <= contract.GracePeriodDays) return 0m;
        return principal * contract.DailyInterestRate;
    }

    public static decimal CalculateOverLimitInterest(
        decimal principal, decimal approvedOverdraftLimit, Contract contract)
    {
        var excess = principal - approvedOverdraftLimit;
        if (excess <= 0) return 0m;

        return excess * contract.DailyOverLimitInterestRate;
    }

    public static decimal CalculateLatePaymentInterest(decimal principal, Contract contract)
    {
        if (principal <= 0) return 0m;
        return principal * contract.DailyLatePaymentInterestRate;
    }

    public static decimal CalculateLatePaymentPenalty(decimal rolloverBalance, Contract contract)
    {
        return rolloverBalance < 0
            ? rolloverBalance * contract.LatePaymentPenaltyRate
            : 0m;
    }
}
