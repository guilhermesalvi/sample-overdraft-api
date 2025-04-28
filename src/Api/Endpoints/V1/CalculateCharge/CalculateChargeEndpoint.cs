using Application.UsesCases.CalculateCharge;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.V1.CalculateCharge;

public static class CalculateChargeEndpoint
{
    public static void MapCalculateChargeEndpoint(this RouteGroupBuilder builder)
    {
        builder
            .MapPost("calculate", CalculateCharge)
            .Accepts<CalculateChargeInput>("application/json")
            .Produces<MonthlyChargeSnapshot>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("CalculateCharges")
            .WithSummary("Calculate Charges to a given usage entry");
    }

    private static async Task<IResult> CalculateCharge(
        [FromBody] CalculateChargeInput request,
        [FromServices] CalculateChargeUseCase useCase,
        CancellationToken cancellationToken,
        [FromQuery] bool dryRun = true)
    {
        return dryRun
            ? TypedResults.Ok(await useCase.ExecuteAsync(request, cancellationToken))
            : TypedResults.BadRequest("Unable to calculate charges without dryRun");
    }
}
