using Domain.Models;

namespace Domain.Calculation;

public static class LatePaymentPenaltyPolicy
{
    public static decimal Calculate(
        bool hasPenaltyBeenApplied, decimal rolloverBalance, Contract contract)
    {
        return IsEligibleForPenalty()
            ? rolloverBalance * contract.LatePaymentPenaltyRate
            : 0m;

        bool IsEligibleForPenalty() =>
            !hasPenaltyBeenApplied && rolloverBalance > 0;
    }
}
