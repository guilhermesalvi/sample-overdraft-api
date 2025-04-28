using Domain.Models;

namespace Domain.Calculation;

public static class MonthlyChargePolicy
{
    public static MonthlyChargeSnapshot Calculate(
        List<DailyLimitUsageEntry> limits, Account account, Contract contract)
    {
        var rolloverBalance = limits.LastOrDefault()?.PrincipalAmount ?? 0m;
        var usedDays = UsedDaysPolicy.Calculate(limits);

        var totalRegularInterest = RegularInterestPolicy.Calculate(limits, account, contract);
        var totalOverLimitInterest = OverLimitInterestPolicy.Calculate(limits, account, contract);
        var totalOverLimitFixedFee = OverLimitFixedFeePolicy.Calculate(limits, account, contract);
        var totalLatePaymentInterest = LatePaymentInterestPolicy.Calculate(limits, contract);
        var totalLatePaymentPenalty = LatePaymentPenaltyPolicy.Calculate(false, limits, contract, account);

        var (totalCreditTax, totalFixedCreditTax) = account.PersonType switch
        {
            PersonType.NaturalPerson => (
                BrazilianNaturalPersonCreditTaxPolicy.CalculateTotalDailyCreditTax(limits),
                BrazilianNaturalPersonCreditTaxPolicy.CalculateMonthlyFixedCreditTax(limits, false)
            ),
            PersonType.LegalPerson => (
                BrazilianLegalPersonCreditTaxPolicy.CalculateTotalDailyCreditTax(limits),
                BrazilianLegalPersonCreditTaxPolicy.CalculateMonthlyFixedCreditTax(limits, false)
            ),
            _ => (0m, 0m)
        };

        var totalDue =
            totalRegularInterest +
            totalOverLimitInterest +
            totalOverLimitFixedFee +
            totalLatePaymentInterest +
            totalLatePaymentPenalty +
            totalCreditTax +
            totalFixedCreditTax;

        var capitalizedPrincipal = CapitalizationPolicy.Calculate(rolloverBalance, totalDue);

        return new MonthlyChargeSnapshot
        {
            AccountId = account.Id,
            ContractId = contract.Id,
            ReferenceDate = limits.First().ReferenceDate,

            ApprovedOverdraftLimit = account.ApprovedOverdraftLimit,
            UsedDaysInCurrentCycle = usedDays,
            GracePeriodDays = contract.GracePeriodDays,

            TotalRegularInterestDue = totalRegularInterest,
            TotalOverLimitInterestDue = totalOverLimitInterest,
            TotalOverLimitFixedFeeDue = totalOverLimitFixedFee,
            TotalLatePaymentInterestDue = totalLatePaymentInterest,
            TotalLatePaymentPenaltyDue = totalLatePaymentPenalty,
            TotalCreditTaxDue = totalCreditTax,
            TotalFixedCreditTaxDue = totalFixedCreditTax,

            TotalDue = totalDue,
            CapitalizedPrincipal = capitalizedPrincipal
        };
    }
}
