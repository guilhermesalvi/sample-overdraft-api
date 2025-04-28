using Domain.Models;

namespace Domain.Calculation;

public static class LatePaymentPenaltyPolicy
{
    public static decimal Calculate(
        bool hasPenaltyBeenApplied, List<DailyLimitUsageEntry> limits, Contract contract, Account account)
    {
        var lastLimit = limits
            .OrderBy(x => x.ReferenceDate)
            .Last();

        var rolloverBalance = lastLimit.PrincipalAmount > 0
            ? lastLimit.PrincipalAmount - account.ApprovedOverdraftLimit
            : 0m;

        return IsEligibleForPenalty()
            ? rolloverBalance * contract.LatePaymentPenaltyRate
            : 0m;

        bool IsEligibleForPenalty() =>
            !hasPenaltyBeenApplied && rolloverBalance > 0;
    }
}
