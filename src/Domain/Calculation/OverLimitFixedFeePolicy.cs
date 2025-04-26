using Domain.Models;

namespace Domain.Calculation;

public static class OverLimitFixedFeePolicy
{
    public static decimal Calculate(
        List<DailyLimitUsageEntry> limits, Account account, Contract contract)
    {
        return IsEligibleForFixedFee()
            ? contract.OverLimitFixedFee
            : 0m;

        bool IsEligibleForFixedFee() =>
            limits.Any(entry => entry.PrincipalAmount > account.ApprovedOverdraftLimit);
    }
}
