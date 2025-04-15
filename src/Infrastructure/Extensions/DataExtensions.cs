using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Overdraft.Domain.Models;
using Overdraft.Infrastructure.Contexts;
using Overdraft.Infrastructure.Repositories;
using Overdraft.Infrastructure.Settings;

namespace Overdraft.Infrastructure.Extensions;

public static class DataExtensions
{
    public static IServiceCollection AddData(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddContexts(configuration)
            .AddCaching(configuration)
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

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(settings.ConnectionString));

        return services;
    }

    private static IServiceCollection AddCaching(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<CachingSettings>()
            .BindConfiguration(nameof(CachingSettings))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var settings = configuration.Get<CachingSettings>() ??
                       throw new InvalidOperationException($"{nameof(CachingSettings)} is required");

        services.AddDistributedSqlServerCache(options =>
        {
            options.ConnectionString = settings.ConnectionString;
            options.SchemaName = settings.SchemaName;
            options.TableName = settings.TableName;
        });

        return services;
    }

    private static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        return services
            .AddScoped<IAccountRepository, AccountRepository>()
            .AddScoped<IContractAgreementRepository, ContractAgreementRepository>()
            .AddScoped<IContractRepository, ContractRepository>()
            .AddScoped<IDailyLimitUsageRepository, DailyLimitUsageRepository>();
    }
}
