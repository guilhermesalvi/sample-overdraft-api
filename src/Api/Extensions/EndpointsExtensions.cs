using Asp.Versioning;
using Overdraft.Api.Endpoints.V1;

namespace Overdraft.Api.Extensions;

public static class EndpointsExtensions
{
    public static void MapEndpoints(this WebApplication app)
    {
        var versionSet = app
            .NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        var routerV1 = app
            .MapGroup("/api/v{version:apiVersion}")
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1, 0);

        routerV1.MapAccountEndpoints();
        routerV1.MapContractEndpoints();
        routerV1.MapDailyLimitEndpoints();
        routerV1.MapPaymentChargeEndpoints();
    }
}
