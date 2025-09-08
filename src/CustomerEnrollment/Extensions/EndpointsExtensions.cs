using Asp.Versioning;
using CustomerEnrollment.Features.BankAccounts.Endpoints.CreateBankAccount;

namespace CustomerEnrollment.Extensions;

public static class EndpointsExtensions
{
    public static void AddEndpoints(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
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

        var bankAccountsV1 = routerV1.MapGroup("bank-accounts");
        bankAccountsV1.MapCreateBankAccountEndpoint();
    }
}
