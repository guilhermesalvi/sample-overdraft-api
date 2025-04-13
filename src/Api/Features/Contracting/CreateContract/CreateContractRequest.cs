using Overdraft.Api.Models;

namespace Overdraft.Api.Features.Contracting.CreateContract;

public record CreateContractRequest(
    int GracePeriodDays,
    decimal MonthlyInterestRate,
    decimal MonthlyIofTax,
    decimal MonthlyOverLimitInterestRate,
    decimal MonthlyLatePaymentInterestRate,
    decimal LatePaymentPenaltyRate)
{
    public Contract ToContract()
    {
        return new Contract
        {
            GracePeriodDays = GracePeriodDays,
            MonthlyInterestRate = MonthlyInterestRate,
            MonthlyIofTax = MonthlyIofTax,
            MonthlyOverLimitInterestRate = MonthlyOverLimitInterestRate,
            MonthlyLatePaymentInterestRate = MonthlyLatePaymentInterestRate,
            LatePaymentPenaltyRate = LatePaymentPenaltyRate,
            IsContractActive = true
        };
    }
}
