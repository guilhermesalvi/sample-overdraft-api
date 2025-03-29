using MediatR;
using Overdraft.Domain.Accounts;

namespace Overdraft.Application.UseCases.GetDailyLimit;

public class GetDailyLimitUseCase(
    IDailyLimitRepository repository) : IRequestHandler<GetDailyLimitInput, IEnumerable<DailyLimit>>
{
    public async Task<IEnumerable<DailyLimit>> Handle(
        GetDailyLimitInput request, CancellationToken cancellationToken)
    {
        var dailyLimits = await repository.GetByReferenceDateAsync(
            request.AccountId,
            request.StartDate,
            request.EndDate,
            cancellationToken);

        return dailyLimits;
    }
}
