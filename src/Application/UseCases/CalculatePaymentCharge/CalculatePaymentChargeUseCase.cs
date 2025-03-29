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
        var account = await accountRepository.GetBydIdAsync(request.AccountId, cancellationToken);

        if (account is null)
        {
            notificationManager.AddNotification("AccountNotFound");
            return null!;
        }

        var contract = await contractRepository.GetByIdAsync(account.ContractId, cancellationToken);

        if (contract is null)
        {
            notificationManager.AddNotification("ContractNotFound");
            return null!;
        }

        var startDate = FirstDayOfPreviousMonth(request.ReferenceDate);
        var endDate = LastDayOfCurrentMonth(request.ReferenceDate);

        var dailyLimits = await dailyLimitRepository.GetByReferenceDateAsync(
            request.AccountId,
            startDate,
            endDate,
            cancellationToken);

        var paymentCharge = PaymentChargeService.CalculatePaymentCharge(
            account,
            contract,
            dailyLimits,
            request.ReferenceDate);

        return paymentCharge;
    }

    private static DateTimeOffset FirstDayOfPreviousMonth(DateTimeOffset referenceMonth)
    {
        return new DateTimeOffset(referenceMonth.Year, referenceMonth.Month, 1, 0, 0, 0, referenceMonth.Offset)
            .AddMonths(-1);
    }

    private static DateTimeOffset LastDayOfCurrentMonth(DateTimeOffset referenceMonth)
    {
        var lastDay = DateTime.DaysInMonth(referenceMonth.Year, referenceMonth.Month);
        return new DateTimeOffset(referenceMonth.Year, referenceMonth.Month, lastDay, 23, 59,
            59, referenceMonth.Offset);
    }
}
