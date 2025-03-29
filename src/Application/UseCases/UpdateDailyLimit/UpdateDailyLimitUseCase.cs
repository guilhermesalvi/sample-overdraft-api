using GSalvi.Toolkit.Notifications;
using MediatR;
using Overdraft.Domain.Accounts;

namespace Overdraft.Application.UseCases.UpdateDailyLimit;

public class UpdateDailyLimitUseCase(
    IAccountRepository accountRepository,
    IDailyLimitRepository dailyLimitRepository,
    LocalizedNotificationManager notificationManager) : IRequestHandler<UpdateDailyLimitInput>
{
    public async Task Handle(UpdateDailyLimitInput request, CancellationToken cancellationToken)
    {
        foreach (var item in request.Items)
        {
            var account = await accountRepository.GetBydIdAsync(
                item.AccountId,
                cancellationToken);

            if (account is null)
            {
                notificationManager.AddNotification("AccountNotFound");
                return;
            }

            var dailyLimit = (await dailyLimitRepository.GetByReferenceDateAsync(
                item.AccountId,
                item.ReferenceDate,
                item.ReferenceDate,
                cancellationToken)).FirstOrDefault();

            if (dailyLimit is not null)
            {
                await dailyLimitRepository.DeleteAsync(dailyLimit, cancellationToken);
            }

            dailyLimit = DailyLimitService.UpdateUsedLimit(new DailyLimit
            {
                AccountId = item.AccountId,
                ReferenceDate = item.ReferenceDate.Date
            }, item.CurrentBalance);

            await dailyLimitRepository.CreateAsync(dailyLimit, cancellationToken);

            var usedDays = await dailyLimitRepository.GetUsedDaysAsync(
                account.Id, item.ReferenceDate, cancellationToken);

            account = account with
            {
                UsedLimit = dailyLimit.UsedLimit,
                UsedDays = usedDays
            };

            await accountRepository.UpdateAsync(account, cancellationToken);
        }
    }
}
