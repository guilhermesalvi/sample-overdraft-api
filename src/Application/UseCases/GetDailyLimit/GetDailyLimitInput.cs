using MediatR;
using Overdraft.Domain.Accounts;

namespace Overdraft.Application.UseCases.GetDailyLimit;

public record GetDailyLimitInput(
    Guid? AccountId,
    DateTimeOffset? StartDate,
    DateTimeOffset? EndDate) : IRequest<IEnumerable<DailyLimit>>;
