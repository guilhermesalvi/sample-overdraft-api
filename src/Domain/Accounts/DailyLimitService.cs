namespace Overdraft.Domain.Accounts;

public static class DailyLimitService
{
    public static DailyLimit UpdateUsedLimit(
        DailyLimit dailyLimit, decimal currentBalance)
    {
        return dailyLimit with
        {
            UsedLimit = currentBalance < 0 ? Math.Abs(currentBalance) : 0
        };
    }
}
