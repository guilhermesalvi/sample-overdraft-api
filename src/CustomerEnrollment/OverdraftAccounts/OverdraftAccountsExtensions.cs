using System.Text.Json.Serialization;
using Asp.Versioning;
using CustomerEnrollment.OverdraftAccounts.Aggregates;
using CustomerEnrollment.OverdraftAccounts.Endpoints.CreateOverdraftAccount;
using CustomerEnrollment.OverdraftAccounts.Endpoints.GetOverdraftAccounts;
using FluentValidation;

namespace CustomerEnrollment.OverdraftAccounts;

public static class OverdraftAccountsExtensions
{
    public static void AddOverdraftAccounts(this WebApplicationBuilder builder)
    {
        builder.Services.ConfigureHttpJsonOptions(options =>
            options.SerializerOptions.TypeInfoResolverChain.Add(OverdraftAccountsJsonContext.Default));

        builder.Services.AddScoped<IValidator<CreateOverdraftAccountRequest>, CreateOverdraftAccountRequestValidator>();
    }

    public static void MapOverdraftAccountEndpoints(this WebApplication app)
    {
        var versionSet = app
            .NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        var routerV1 = app
            .MapGroup("/api/v{version:apiVersion}/overdraft-accounts")
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1, 0);

        routerV1.MapCreateOverdraftAccountEndpoint();
        routerV1.MapGetOverdraftAccountsEndpoint();
    }
}

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    UseStringEnumConverter = true
)]
[JsonSerializable(typeof(OverdraftAccount))]
[JsonSerializable(typeof(CustomerType))]
[JsonSerializable(typeof(CreateOverdraftAccountRequest))]
internal partial class OverdraftAccountsJsonContext : JsonSerializerContext;
