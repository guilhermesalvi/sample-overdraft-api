using GSalvi.Toolkit.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Overdraft.Application.UseCases.CalculatePaymentCharge;
using Overdraft.Domain.Accounts;

namespace Overdraft.Api.Endpoints.V1;

public static class PaymentChargeEndpoints
{
    public static void MapPaymentChargeEndpoints(this RouteGroupBuilder builder)
    {
        var group = builder
            .MapGroup("/payment-charge");

        group
            .MapPost("", async (
                [FromServices] IMediator mediator,
                [FromServices] LocalizedNotificationManager notificationManager,
                CalculatePaymentChargeInput request,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(request, cancellationToken);

                return notificationManager.HasNotifications
                    ? notificationManager.Notifications.ToValidationProblem()
                    : Results.Ok(result);
            })
            .Accepts<CalculatePaymentChargeInput>("application/json")
            .Produces<PaymentCharge>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("CalculatePaymentCharge")
            .WithTags("payment-charge")
            .WithSummary("Calculates the payment charge for an account.");
    }
}
