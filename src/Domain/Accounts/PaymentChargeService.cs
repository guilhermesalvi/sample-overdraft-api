namespace Overdraft.Domain.Accounts;

public static class PaymentChargeService
{
    public static PaymentCharge CalculateCharge(
        Account account,
        Contract contract,
        DateTimeOffset referenceDate,
        (List<DailyLimit> LastMonth, List<DailyLimit> CurrentMonth) limits)
    {
        var (lastMonthLimits, currentMonthLimits) = limits;

        var usedDays = CountUsedDays(currentMonthLimits);
        var iofTaxDue = CalculateIofTax(contract, currentMonthLimits);
        var interestDue = CalculateInterestDue(contract, usedDays, currentMonthLimits);
        var overLimitInterestDue = CalculateOverLimitInterestDue(account, contract, currentMonthLimits);
        var latePaymentInterestDue = CalculateLatePaymentInterestDue(contract, lastMonthLimits, currentMonthLimits);
        var latePaymentPenaltyDue = CalculateLatePaymentPenaltyDue(contract, lastMonthLimits);
        var totalDue = interestDue + iofTaxDue + overLimitInterestDue + latePaymentInterestDue + latePaymentPenaltyDue;

        return new PaymentCharge
        {
            AccountId = account.Id,
            ContractId = contract.Id,
            GracePeriodDays = contract.GracePeriodDays,
            UsedDays = usedDays,
            InterestDue = interestDue,
            IofTaxDue = iofTaxDue,
            OverLimitInterestDue = overLimitInterestDue,
            LatePaymentInterestDue = latePaymentInterestDue,
            LatePaymentPenaltyDue = latePaymentPenaltyDue,
            TotalDue = totalDue,
            ReferenceDate = referenceDate,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    private static int CountUsedDays(List<DailyLimit> limits) =>
        limits.Count(x => x.UsedLimit > 0m);

    private static decimal CalculateIofTax(Contract contract, List<DailyLimit> limits) =>
        limits.Sum(x => x.UsedLimit * contract.DailyIofTax);

    private static decimal CalculateInterestDue(
        Contract contract, int usedDays, List<DailyLimit> limits) =>
        usedDays > contract.GracePeriodDays
            ? limits.Sum(x => x.UsedLimit * contract.DailyInterestRate)
            : 0m;

    private static decimal CalculateOverLimitInterestDue(
        Account account, Contract contract, List<DailyLimit> limits)
        => limits
            .Where(x => x.UsedLimit > account.OverdraftLimit)
            .Sum(x => (x.UsedLimit - account.OverdraftLimit) * contract.DailyOverLimitInterestRate);

    private static decimal CalculateLatePaymentPenaltyDue(
        Contract contract, List<DailyLimit> limits) =>
        limits.LastOrDefault() is { UsedLimit: > 0m } last
            ? last.UsedLimit * contract.LatePaymentPenaltyRate
            : 0m;

    private static decimal CalculateLatePaymentInterestDue(
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
