using Microsoft.AspNetCore.Mvc;
using Overdraft.Api.Data;
using Overdraft.Api.SeedWork.Filters;

namespace Overdraft.Api.Features.Transactional.CalculatePaymentCharge;

public static class CalculatePaymentChargeEndpoint
{
    public static void MapCalculatePaymentChargeEndpoint(this RouteGroupBuilder builder)
    {
        builder
            .MapPost("payment-charge", CalculatePaymentCharge)
            .AddEndpointFilter(new ValidationFilter<CalculatePaymentChargeRequest>())
            .Accepts<CalculatePaymentChargeRequest>("application/json")
            .Produces<CalculatePaymentChargeResponse>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("CalculatePaymentCharge")
            .WithSummary("Calculates the payment charge for a given account.");
    }

    private static async Task<IResult> CalculatePaymentCharge(
        [FromBody] CalculatePaymentChargeRequest request,
        [FromServices] AccountRepository accountRepository,
        [FromServices] ContractRepository contractRepository,
        [FromServices] DailyLimitRepository dailyLimitRepository,
        CancellationToken cancellationToken)

    {
        var account = (await accountRepository.GetAsync(request.AccountId, cancellationToken)).First();
        var contract = (await contractRepository.GetAsync(account.ContractId, cancellationToken)).First();

        var startDate = request.ReferenceDate.AddMonths(-1);
        var endDate = request.ReferenceDate;
        var limits = await dailyLimitRepository.GetByAccountIdAndRangeDatesAsync(
            request.AccountId, startDate, endDate, cancellationToken);

        var lastMonthLimits = limits.Where(x => x.ReferenceDate.Month == startDate.Month).ToList();
        var currentMonthLimits =
            limits.Where(x => x.ReferenceDate.Month == request.ReferenceDate.Month).ToList();

        var usedDays = PaymentChargeCalculator.CountUsedDays(currentMonthLimits);
        var iofTaxDue = PaymentChargeCalculator.CalculateIofTaxDue(contract, currentMonthLimits);
        var interestDue = PaymentChargeCalculator.CalculateInterestDue(contract, usedDays, currentMonthLimits);
        var overLimitInterestDue = PaymentChargeCalculator.CalculateOverLimitInterestDue(
            account, contract, currentMonthLimits);
        var latePaymentInterestDue = PaymentChargeCalculator.CalculateLatePaymentInterestDue(
            contract, lastMonthLimits, currentMonthLimits);
        var latePaymentPenaltyDue = PaymentChargeCalculator.CalculateLatePaymentPenaltyDue(
            contract, lastMonthLimits);
        var totalDue = iofTaxDue + interestDue + overLimitInterestDue + latePaymentInterestDue +
                       latePaymentPenaltyDue;

        var paymentCharge = new CalculatePaymentChargeResponse(
            account.Id,
            contract.Id,
            contract.GracePeriodDays,
            usedDays,
            interestDue,
            iofTaxDue,
            overLimitInterestDue,
            latePaymentInterestDue,
            latePaymentPenaltyDue,
            totalDue,
            request.ReferenceDate);

        return Results.Ok(paymentCharge);
    }
}
