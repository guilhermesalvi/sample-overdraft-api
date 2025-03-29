using MediatR;
using Overdraft.Domain.Accounts;

namespace Overdraft.Application.UseCases.CreateContract;

public record CreateContractInput(
    int GracePeriodDays,
    decimal MonthlyInterestRate,
    decimal MonthlyIofTax,
    decimal MonthlyOverLimitInterestRate,
    decimal MonthlyLatePaymentInterestRate,
    decimal LatePaymentPenaltyRate) : IRequest<Contract>;
