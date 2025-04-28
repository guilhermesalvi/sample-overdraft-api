using Domain.Models;

namespace Domain.Calculation;

public static class UsedDaysPolicy
{
    public static int Calculate(List<DailyLimitUsageEntry> limits)
    {
        return limits.Count(x => x.PrincipalAmount > 0);
    }
}
