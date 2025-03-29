using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Overdraft.Domain.Accounts;
using Overdraft.Infrastructure.Data.Context;
using Overdraft.Infrastructure.Data.Repositories;
using Overdraft.Infrastructure.Data.Settings;

namespace Overdraft.Infrastructure.Data.Extensions;

public static class DataExtensions
{
    public static void AddData(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddContexts(configuration)
            .AddRepositories();
    }

    private static IServiceCollection AddContexts(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<DataSettings>()
            .BindConfiguration(nameof(DataSettings))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var settings = configuration.Get<DataSettings>() ??
                       throw new InvalidOperationException($"{nameof(DataSettings)} is required");

        // services.AddDbContext<ApplicationDbContext>(options =>
        //     options.UseSqlServer(settings.ConnectionString));

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("LoanOverdraft"));

        return services;
    }

    private static void AddRepositories(
        this IServiceCollection services)
    {
        services
            .AddScoped<IDailyLimitRepository, DailyLimitRepository>()
            .AddScoped<IAccountRepository, AccountRepository>()
            .AddScoped<IContractRepository, ContractRepository>();
    }
}
