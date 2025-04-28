using System.Text.Json;
using System.Text.Json.Serialization;
using Api.Endpoints.V1.CalculateCharge;
using Asp.Versioning;

namespace Api.Extensions;

public static class EndpointsExtensions
{
    public static void AddEndpoints(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.ConfigureHttpJsonOptions(opts =>
        {
            var jsonOpts = opts.SerializerOptions;
            jsonOpts.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            jsonOpts.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        });
    }

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

        var chargeV1Router = routerV1
            .MapGroup("charge")
            .WithTags("charge");

        chargeV1Router.MapCalculateChargeEndpoint();
    }
}
