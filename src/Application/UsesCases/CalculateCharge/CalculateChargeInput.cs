using Domain.Models;

namespace Application.UsesCases.CalculateCharge;

public record CalculateChargeInput(
    CalculateChargeAccount Account,
    CalculateChargeContract Contract,
    List<CalculateChargeUsageEntry> Entries);

public record CalculateChargeAccount(
    PersonType PersonType,
    decimal CustomerAssetsHeld,
    decimal ApprovedOverdraftLimit)
{
    public Account ToAccount()
    {
        return new Account
        {
            Id = Guid.Empty,
            PersonType = PersonType,
            CustomerAssetsHeld = CustomerAssetsHeld,
            ApprovedOverdraftLimit = ApprovedOverdraftLimit
        };
    }
}

public record CalculateChargeContract(
    int GracePeriodDays,
    decimal MonthlyInterestRate,
    decimal MonthlyOverLimitInterestRate,
    decimal OverLimitFixedFee,
    decimal MonthlyLatePaymentInterestRate,
    decimal LatePaymentPenaltyRate)
{
    public Contract ToContract()
    {
        return new Contract
        {
            Id = Guid.Empty,
            GracePeriodDays = GracePeriodDays,
            MonthlyInterestRate = MonthlyInterestRate,
            MonthlyOverLimitInterestRate = MonthlyOverLimitInterestRate,
            OverLimitFixedFee = OverLimitFixedFee,
            MonthlyLatePaymentInterestRate = MonthlyLatePaymentInterestRate,
            LatePaymentPenaltyRate = LatePaymentPenaltyRate
        };
    }
}

public record CalculateChargeUsageEntry(
    DateOnly ReferenceDate,
    decimal PrincipalAmount)
{
    public DailyLimitUsageEntry ToDailyLimitUsageEntry()
    {
        return new DailyLimitUsageEntry
        {
            ReferenceDate = ReferenceDate,
            PrincipalAmount = PrincipalAmount
        };
    }
}
