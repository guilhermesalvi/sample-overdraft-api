namespace Overdraft.Api.Features.Transactional.CalculatePaymentCharge;

public record CalculatePaymentChargeResponse(
    Guid AccountId,
    Guid ContractId,
    int GracePeriodDays,
    int UsedDays,
    decimal InterestDue,
    decimal IofTaxDue,
    decimal OverLimitInterestDue,
    decimal LatePaymentInterestDue,
    decimal LatePaymentPenaltyDue,
    decimal TotalDue,
    DateOnly ReferenceDate);
