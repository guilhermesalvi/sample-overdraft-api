using Overdraft.Api.Models;

namespace Overdraft.Api.Features.Accounting.CalculatePaymentCharge;

public static class PaymentChargeCalculator
{
    public static int CountUsedDays(List<DailyLimit> limits) =>
        limits.Count(x => x.UsedLimit > 0m);

    public static decimal CalculateIofTaxDue(Contract contract, List<DailyLimit> limits) =>
        limits.Sum(x => x.UsedLimit * contract.DailyIofTax);

    public static decimal CalculateInterestDue(
        Contract contract, int usedDays, List<DailyLimit> limits) =>
        usedDays > contract.GracePeriodDays
            ? limits.Sum(x => x.UsedLimit * contract.DailyInterestRate)
            : 0m;

    public static decimal CalculateOverLimitInterestDue(
        Account account, Contract contract, List<DailyLimit> limits)
        => limits
            .Where(x => x.UsedLimit > account.OverdraftLimit)
            .Sum(x => (x.UsedLimit - account.OverdraftLimit) * contract.DailyOverLimitInterestRate);

    public static decimal CalculateLatePaymentPenaltyDue(
        Contract contract, List<DailyLimit> limits) =>
        limits.LastOrDefault() is { UsedLimit: > 0m } last
            ? last.UsedLimit * contract.LatePaymentPenaltyRate
            : 0m;

    public static decimal CalculateLatePaymentInterestDue(
        Contract contract,
        List<DailyLimit> lastMonthLimits,
        List<DailyLimit> currentMonthLimits)
    {
        if (lastMonthLimits.LastOrDefault() is not { UsedLimit: > 0m } lastDay)
            return 0m;

        var rate = contract.DailyLatePaymentInterestRate;
        var firstDay = lastDay.UsedLimit * rate;

        var accrued = currentMonthLimits
            .TakeWhile(x => x.UsedLimit > 0m)
            .Sum(x => x.UsedLimit * rate);

        return firstDay + accrued;
    }
}
