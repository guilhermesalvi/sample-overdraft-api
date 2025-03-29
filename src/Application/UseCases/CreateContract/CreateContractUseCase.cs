using MediatR;
using Overdraft.Domain.Accounts;

namespace Overdraft.Application.UseCases.CreateContract;

public class CreateContractUseCase(
    IContractRepository repository) : IRequestHandler<CreateContractInput, Contract>
{
    public async Task<Contract> Handle(
        CreateContractInput request, CancellationToken cancellationToken)
    {
        var contract = new Contract
        {
            GracePeriodDays = request.GracePeriodDays,
            MonthlyInterestRate = request.MonthlyInterestRate,
            MonthlyIofTax = request.MonthlyIofTax,
            MonthlyOverLimitInterestRate = request.MonthlyOverLimitInterestRate,
            MonthlyLatePaymentInterestRate = request.MonthlyLatePaymentInterestRate,
            LatePaymentPenaltyRate = request.LatePaymentPenaltyRate
        };

        contract = contract with { IsContractActive = true };

        await repository.CreateAsync(contract, cancellationToken);

        return contract;
    }
}
