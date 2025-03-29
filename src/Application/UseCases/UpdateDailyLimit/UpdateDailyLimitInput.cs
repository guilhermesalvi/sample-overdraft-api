using MediatR;

namespace Overdraft.Application.UseCases.UpdateDailyLimit;

public record UpdateDailyLimitInput(
    IEnumerable<UpdateDailyLimitInputItem> Items) : IRequest;

public record UpdateDailyLimitInputItem(
    Guid AccountId,
    decimal CurrentBalance,
    DateTimeOffset ReferenceDate);
