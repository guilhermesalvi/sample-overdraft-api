using Domain.Models;

namespace Domain.Calculation;

public static class LatePaymentInterestPolicy
{
    public static decimal Calculate(List<DailyLimitUsageEntry> limits, Contract contract)
    {
        var lastLimit = limits
            .OrderBy(x => x.ReferenceDate)
            .Last();

        return lastLimit.PrincipalAmount > 0
            ? lastLimit.PrincipalAmount * contract.DailyLatePaymentInterestRate
            : 0m;
    }
}
