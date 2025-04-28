using Application.UsesCases.CalculateCharge;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ApplicationExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CalculateChargeUseCase>();
    }
}
