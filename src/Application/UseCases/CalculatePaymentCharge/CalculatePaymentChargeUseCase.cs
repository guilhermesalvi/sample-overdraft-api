using GSalvi.Toolkit.Notifications;
using MediatR;
using Overdraft.Domain.Accounts;

namespace Overdraft.Application.UseCases.CalculatePaymentCharge;

public class CalculatePaymentChargeUseCase(
    IAccountRepository accountRepository,
    IContractRepository contractRepository,
    IDailyLimitRepository dailyLimitRepository,
    LocalizedNotificationManager notificationManager) : IRequestHandler<CalculatePaymentChargeInput, PaymentCharge>
{
    public async Task<PaymentCharge> Handle(CalculatePaymentChargeInput request, CancellationToken cancellationToken)
    {
        var result = await GetAccountAndContract(request, cancellationToken);
        if (result is null) return null!;

        var (account, contract) = result.Value;

        var limits = await GetLimits(request, cancellationToken);
        var charge = PaymentChargeService.CalculateCharge(account, contract, request.ReferenceDate, limits);

        return charge;
    }

    private async Task<(Account Account, Contract Contract)?> GetAccountAndContract(
        CalculatePaymentChargeInput request,
        CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetBydIdAsync(request.AccountId, cancellationToken);
        if (account is null)
        {
            notificationManager.AddNotification("AccountNotFound");
            return null;
        }

        var contract = await contractRepository.GetByIdAsync(account.ContractId, cancellationToken);
        if (contract is null)
        {
            notificationManager.AddNotification("ContractNotFound");
            return null;
        }

        return (account, contract);
    }

    private async Task<(List<DailyLimit> LastMonth, List<DailyLimit> CurrentMonth)> GetLimits(
        CalculatePaymentChargeInput request,
        CancellationToken cancellationToken)
    {
        var lastMonth = request.ReferenceDate.AddMonths(-1);
        var currentMonth = request.ReferenceDate;

        var lastMonthLimitsTask = dailyLimitRepository.GetByReferenceDateAsync(
            request.AccountId, FirstDay(lastMonth), LastDay(lastMonth), cancellationToken);

        var currentMonthLimitsTask = dailyLimitRepository.GetByReferenceDateAsync(
            request.AccountId, FirstDay(currentMonth), LastDay(currentMonth), cancellationToken);

        await Task.WhenAll(lastMonthLimitsTask, currentMonthLimitsTask);

        return (await lastMonthLimitsTask, await currentMonthLimitsTask);
    }

    private static DateTimeOffset FirstDay(DateTimeOffset referenceMonth) =>
        new(referenceMonth.Year, referenceMonth.Month, 1, 0, 0, 0, referenceMonth.Offset);

    private static DateTimeOffset LastDay(DateTimeOffset referenceMonth)
    {
        var lastDay = DateTime.DaysInMonth(referenceMonth.Year, referenceMonth.Month);
        return new DateTimeOffset(referenceMonth.Year, referenceMonth.Month, lastDay, 23, 59, 59,
            referenceMonth.Offset);
    }
}
