using Asp.Versioning;
using FluentValidation;
using Overdraft.Api.Features.Accounting.CalculatePaymentCharge;
using Overdraft.Api.Features.Accounting.CreateAccount;
using Overdraft.Api.Features.Contracting.CreateContract;
using Overdraft.Api.Features.Accounting.DeleteAccount;
using Overdraft.Api.Features.Contracting.DeleteContract;
using Overdraft.Api.Features.Accounting.GetAccount;
using Overdraft.Api.Features.Accounting.GetDailyLimit;
using Overdraft.Api.Features.Accounting.UpdateDailyLimit;
using Overdraft.Api.Features.Contracting.GetContract;

namespace Overdraft.Api.Extensions;

public static class EndpointsExtensions
{
    public static void AddEndpoints(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddFilters();
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

        routerV1.MapContractV1Endpoints();
        routerV1.MapAccountV1Endpoints();
    }

    private static RouteGroupBuilder MapContractV1Endpoints(this RouteGroupBuilder builder)
    {
        var router = builder
            .MapGroup("contract")
            .WithTags("contract");

        router.MapCreateContractEndpoint();
        router.MapDeleteContractEndpoint();
        router.MapGetContractEndpoint();

        return router;
    }

    private static RouteGroupBuilder MapAccountV1Endpoints(this RouteGroupBuilder builder)
    {
        var router = builder
            .MapGroup("account")
            .WithTags("account");

        router.MapCreateAccountEndpoint();
        router.MapDeleteAccountEndpoint();
        router.MapGetAccountEndpoint();

        router.MapGetDailyLimitEndpoint();
        router.MapUpdateDailyLimitEndpoint();

        router.MapCalculatePaymentChargeEndpoint();

        return router;
    }

    private static void AddFilters(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateAccountRequest>, CreateAccountRequestValidator>();
        services.AddScoped<IValidator<UpdateDailyLimitRequest>, UpdateDailyLimitRequestValidator>();
        services.AddScoped<IValidator<UpdateDailyLimitItem>, UpdateDailyLimitItemValidator>();
        services.AddScoped<IValidator<CalculatePaymentChargeRequest>, CalculatePaymentChargeRequestValidator>();
    }
}
