using Overdraft.Api.Models;

namespace Overdraft.Api.Features.Accounting.UpdateDailyLimit;

public record UpdateDailyLimitRequest(
    List<UpdateDailyLimitItem> Items);

public record UpdateDailyLimitItem(
    Guid AccountId,
    decimal CurrentBalance,
    DateOnly ReferenceDate)
{
    public DailyLimit ToDailyLimit()
    {
        return new DailyLimit
        {
            AccountId = AccountId,
            UsedLimit = CurrentBalance,
            ReferenceDate = ReferenceDate
        };
    }
}
