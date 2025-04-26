using Domain.Models;

namespace Domain.Calculation;

public static class LatePaymentInterestPolicy
{
    public static decimal Calculate(List<DailyLimitUsageEntry> lateDays, Contract contract)
    {
        return lateDays
            .Select(entry => entry.PrincipalAmount)
            .Where(amount => amount > 0)
            .Sum(amount => amount * contract.DailyLatePaymentInterestRate);
    }
}
