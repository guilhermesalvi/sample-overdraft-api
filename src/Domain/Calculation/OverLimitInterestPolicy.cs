using Domain.Models;

namespace Domain.Calculation;

public static class OverLimitInterestPolicy
{
    public static decimal Calculate(
        List<DailyLimitUsageEntry> limits, Account account, Contract contract)
    {
        return limits
            .Select(entry => GetOverLimitAmount(entry.PrincipalAmount, account.ApprovedOverdraftLimit))
            .Where(overLimit => overLimit > 0)
            .Sum(overLimit => overLimit * contract.DailyOverLimitInterestRate);
    }

    private static decimal GetOverLimitAmount(decimal principal, decimal approvedLimit) =>
        principal > approvedLimit ? principal - approvedLimit : 0m;
}
