using Application.Logging;
using Domain.Calculation;
using Domain.Models;
using Serilog.Context;

namespace Application.UsesCases.CalculateCharge;

public class CalculateChargeUseCase
{
    public Task<MonthlyChargeSnapshot> ExecuteAsync(
        CalculateChargeInput input, CancellationToken cancellationToken)
    {
        using (LogContext.Push(new LogEnricherBuilder()
                   .WithProperty("PersonType", input.Account.PersonType.ToString())))
        {
            var account = input.Account.ToAccount();
            var contract = input.Contract.ToContract();
            var limits = input.Entries.Select(x => x.ToDailyLimitUsageEntry()).ToList();

            var charge = MonthlyChargePolicy.Calculate(limits, account, contract);
            charge = charge with
            {
                Id = Guid.Empty,
                TotalRegularInterestDue = Math.Round(charge.TotalRegularInterestDue, 2),
                TotalOverLimitInterestDue = Math.Round(charge.TotalOverLimitInterestDue, 2),
                TotalOverLimitFixedFeeDue = Math.Round(charge.TotalOverLimitFixedFeeDue, 2),
                TotalLatePaymentInterestDue = Math.Round(charge.TotalLatePaymentInterestDue, 2),
                TotalLatePaymentPenaltyDue = Math.Round(charge.TotalLatePaymentPenaltyDue, 2),
                TotalCreditTaxDue = Math.Round(charge.TotalCreditTaxDue, 2),
                TotalFixedCreditTaxDue = Math.Round(charge.TotalFixedCreditTaxDue, 2),
                TotalDue = Math.Round(charge.TotalDue, 2),
                CapitalizedPrincipal = Math.Round(charge.CapitalizedPrincipal, 2),
            };

            return Task.FromResult(charge);
        }
    }
}
