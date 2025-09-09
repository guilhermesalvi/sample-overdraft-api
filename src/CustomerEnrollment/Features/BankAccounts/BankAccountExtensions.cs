using System.Text.Json.Serialization;
using Asp.Versioning;
using CustomerEnrollment.Features.BankAccounts.Domain;
using CustomerEnrollment.Features.BankAccounts.Endpoints.CreateBankAccount;
using CustomerEnrollment.Features.BankAccounts.Endpoints.GetBankAccounts;
using CustomerEnrollment.Features.BankAccounts.Repositories;
using FluentValidation;

namespace CustomerEnrollment.Features.BankAccounts;

public static class BankAccountExtensions
{
    public static void AddBankAccount(this WebApplicationBuilder builder)
    {
        builder.Services.ConfigureHttpJsonOptions(options =>
            options.SerializerOptions.TypeInfoResolverChain.Add(BankAccountsJsonSerializerContext.Default));

        builder.Services.AddScoped<BankAccountRepository>();
        builder.Services.AddScoped<IValidator<CreateBankAccountRequest>, CreateBankAccountRequestValidator>();
    }

    public static void MapBankAccountEndpoints(this WebApplication app)
    {
        var versionSet = app
            .NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        var routerV1 = app
            .MapGroup("/api/v{version:apiVersion}/bank-accounts")
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1, 0);

        routerV1.MapCreateBankAccountEndpoint();
        routerV1.MapGetBankAccountsEndpoint();
    }
}

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    UseStringEnumConverter = true
)]
[JsonSerializable(typeof(BankAccount))]
[JsonSerializable(typeof(CustomerType))]
[JsonSerializable(typeof(CreateBankAccountRequest))]
internal partial class BankAccountsJsonSerializerContext : JsonSerializerContext;
