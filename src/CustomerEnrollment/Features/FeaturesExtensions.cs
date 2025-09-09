using CustomerEnrollment.Features.BankAccounts;
using CustomerEnrollment.Features.OverdraftContracts;

namespace CustomerEnrollment.Features;

public static class FeaturesExtensions
{
    public static void AddFeatures(this WebApplicationBuilder builder)
    {
        builder.AddBankAccount();
        builder.AddOverdraftContract();
    }

    public static void MapFeatureEndpoints(this WebApplication app)
    {
        app.MapBankAccountEndpoints();
        app.MapOverdraftContractEndpoints();
    }
}
