using MediatR;
using Overdraft.Domain.Accounts;

namespace Overdraft.Application.UseCases.CalculatePaymentCharge;

public record CalculatePaymentChargeInput(
    Guid AccountId,
    DateTimeOffset ReferenceDate) : IRequest<PaymentCharge>;
