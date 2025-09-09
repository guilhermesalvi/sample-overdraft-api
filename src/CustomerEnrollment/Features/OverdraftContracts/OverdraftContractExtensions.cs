using System.Text.Json.Serialization;
using Asp.Versioning;
using CustomerEnrollment.Features.OverdraftContracts.Domain;

namespace CustomerEnrollment.Features.OverdraftContracts;

public static class OverdraftContractExtensions
{
    public static void AddOverdraftContract(this WebApplicationBuilder builder)
    {
        builder.Services.ConfigureHttpJsonOptions(options =>
            options.SerializerOptions.TypeInfoResolverChain.Add(OverdraftContractsJsonSerializerContext.Default));
    }

    public static void MapOverdraftContractEndpoints(this WebApplication app)
    {
        var versionSet = app
            .NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        var routerV1 = app
            .MapGroup("/api/v{version:apiVersion}/overdraft-contracts")
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1, 0);
    }
}

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    UseStringEnumConverter = true
)]
[JsonSerializable(typeof(OverdraftContract))]
internal partial class OverdraftContractsJsonSerializerContext : JsonSerializerContext;
