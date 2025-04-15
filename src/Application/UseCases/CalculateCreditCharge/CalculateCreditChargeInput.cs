namespace Overdraft.Application.UseCases.CalculateCreditCharge;

public record CalculateCreditChargeInput(
    Guid AccountId,
    DateOnly ReferenceDate);
