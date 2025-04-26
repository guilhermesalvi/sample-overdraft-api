using Domain.Models;

namespace Domain.Calculation;

public static class RegularInterestPolicy
{
    public static decimal Calculate(
        List<DailyLimitUsageEntry> limits, Account account, Contract contract)
    {
        var positiveEntries = limits.Where(HasPositivePrincipal).ToList();

        if (!ExceedsGracePeriod(positiveEntries, contract.GracePeriodDays))
            return 0m;

        return positiveEntries
            .Select(entry => CappedPrincipal(entry.PrincipalAmount, account.ApprovedOverdraftLimit))
            .Sum(principal => principal * contract.DailyInterestRate);
    }

    private static bool HasPositivePrincipal(DailyLimitUsageEntry entry) =>
        entry.PrincipalAmount > 0;

    private static bool ExceedsGracePeriod(IEnumerable<DailyLimitUsageEntry> entries, int gracePeriodDays) =>
        entries.Count() > gracePeriodDays;

    private static decimal CappedPrincipal(decimal principal, decimal limit) =>
        principal > limit ? limit : principal;
}
