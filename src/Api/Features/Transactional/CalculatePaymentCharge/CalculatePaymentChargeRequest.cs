namespace Overdraft.Api.Features.Transactional.CalculatePaymentCharge;

public record CalculatePaymentChargeRequest(
    Guid AccountId,
    DateOnly ReferenceDate);
