using Domain.Models;

namespace Domain.Calculation;

public static class BrazilianNaturalPersonCreditTaxPolicy
{
    public const decimal DailyTaxRate = 0.000082m;
    public const decimal FixedTaxRate = 0.0038m;

    public static decimal CalculateTotalDailyCreditTax(List<DailyLimitUsageEntry> limits)
    {
        return limits
            .Select(x => x.PrincipalAmount)
            .Where(x => x > 0)
            .Sum(x => x * DailyTaxRate);
    }

    public static decimal CalculateMonthlyFixedCreditTax(
        List<DailyLimitUsageEntry> limits, bool hasAlreadyBeenCharged)
    {
        if (hasAlreadyBeenCharged) return 0m;

        var eligibleValues = limits
            .Select(x => x.PrincipalAmount)
            .Where(x => x > 0);

        var maxPrincipal = eligibleValues
            .DefaultIfEmpty(0)
            .Max();

        return maxPrincipal * FixedTaxRate;
    }
}
