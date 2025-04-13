namespace Overdraft.Api.Features.Accounting.CalculatePaymentCharge;

public record CalculatePaymentChargeRequest(
    Guid AccountId,
    DateOnly ReferenceDate);
