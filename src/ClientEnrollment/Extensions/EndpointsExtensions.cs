using System.Text.Json.Serialization;
using Asp.Versioning;
using ClientEnrollment.BankAccount.Endpoints.CreateBankAccount;
using Microsoft.AspNetCore.Mvc;

namespace ClientEnrollment.Extensions;

public static class EndpointsExtensions
{
    public static void AddEndpoints(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.ConfigureHttpJsonOptions(options =>
            options.SerializerOptions.TypeInfoResolverChain.Insert(0, DefaultJsonSerializerContext.Default));
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

        var bankAccountV1 = routerV1.MapGroup("/bank-accounts");
        bankAccountV1.MapCreateBankAccountEndpoint();
    }
}

[JsonSerializable(typeof(ProblemDetails))]
internal partial class DefaultJsonSerializerContext : JsonSerializerContext;
