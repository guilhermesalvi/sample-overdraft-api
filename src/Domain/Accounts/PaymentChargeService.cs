namespace Overdraft.Domain.Accounts;

public static class PaymentChargeService
{
    public static PaymentCharge CalculatePaymentCharge(
        Account account,
        Contract contract,
        IEnumerable<DailyLimit> dailyLimits,
        DateTimeOffset referenceDate)
    {
        dailyLimits = dailyLimits.ToList();

        var referenceMonthDailyLimits = dailyLimits
            .Where(x => x.ReferenceDate.Month == referenceDate.Month)
            .OrderBy(x => x.ReferenceDate)
            .ToList();

        var lastMonthDailyLimits = dailyLimits
            .Where(x => x.ReferenceDate.Month == referenceDate.AddMonths(-1).Month)
            .OrderBy(x => x.ReferenceDate)
            .ToList();

        var usedDays = CalculateUsedDays(referenceMonthDailyLimits);
        var iofTaxDue = CalculateIofTax(contract, referenceMonthDailyLimits);
        var interestDue = CalculateInterestDue(usedDays, contract, referenceMonthDailyLimits);
        var overLimitInterestDue = CalculateOverLimitInterestDue(account, contract, referenceMonthDailyLimits);
        var latePaymentInterestDue =
            CalculateLatePaymentInterestDue(contract, lastMonthDailyLimits, referenceMonthDailyLimits);
        var latePaymentPenaltyDue =
            CalculateLatePaymentPenaltyDue(contract, lastMonthDailyLimits);

        var totalDue =
            interestDue
            + iofTaxDue
            + overLimitInterestDue
            + latePaymentInterestDue
            + latePaymentPenaltyDue;

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
            TotalDue = totalDue
        };
    }

    private static int CalculateUsedDays(IEnumerable<DailyLimit> referenceMonthDailyLimits) =>
        referenceMonthDailyLimits.Count(x => x.UsedLimit > 0);

    private static decimal CalculateIofTax(Contract contract, IEnumerable<DailyLimit> referenceMonthDailyLimits) =>
        referenceMonthDailyLimits.Sum(x => x.UsedLimit * contract.DailyIofTax);

    private static decimal CalculateInterestDue(
        int usedDays, Contract contract, IEnumerable<DailyLimit> referenceMonthDailyLimits) =>
        usedDays > contract.GracePeriodDays
            ? referenceMonthDailyLimits.Sum(x => x.UsedLimit * contract.DailyInterestRate)
            : 0;

    private static decimal CalculateOverLimitInterestDue(
        Account account, Contract contract, IEnumerable<DailyLimit> referenceMonthDailyLimits) =>
        referenceMonthDailyLimits
            .Where(x => x.UsedLimit > account.OverdraftLimit)
            .Sum(x => (x.UsedLimit - account.OverdraftLimit) * contract.DailyOverLimitInterestRate);

    private static decimal CalculateLatePaymentInterestDue(
        Contract contract,
        IEnumerable<DailyLimit> lastMonthDailyLimits,
        IEnumerable<DailyLimit> referenceMonthDailyLimits)

    {
        var lastDay = lastMonthDailyLimits.Last();
        if (lastDay.UsedLimit == 0) return 0;

        var totalLatePaymentInterestDue =
            lastDay.UsedLimit * contract.DailyLatePaymentInterestRate +
            referenceMonthDailyLimits
                .TakeWhile(dailyLimit => dailyLimit.UsedLimit > 0)
                .Sum(dailyLimit => dailyLimit.UsedLimit * contract.DailyLatePaymentInterestRate);

        return totalLatePaymentInterestDue;
    }

    private static decimal CalculateLatePaymentPenaltyDue(
        Contract contract,
        IEnumerable<DailyLimit> lastMonthDailyLimits)

    {
        var lastDay = lastMonthDailyLimits.Last();
        if (lastDay.UsedLimit == 0) return 0;

        return lastDay.UsedLimit * contract.LatePaymentPenaltyRate;
    }
}
