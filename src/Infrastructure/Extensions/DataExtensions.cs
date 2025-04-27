using Domain.Models;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class DataExtensions
{
    public static IServiceCollection AddData(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddContext(configuration)
            .AddRepositories();
    }

    private static IServiceCollection AddContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<DataSettings>()
            .BindConfiguration(nameof(DataSettings))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var settings = configuration.GetSection(nameof(DataSettings)).Get<DataSettings>()!;

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(settings.ConnectionString));

        services.AddDistributedSqlServerCache(options =>
        {
            options.ConnectionString = settings.ConnectionString;
            options.SchemaName = settings.CacheSchemaName;
            options.TableName = settings.CacheTableName;
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
            .AddScoped<IDailyLimitUsageRepository, DailyLimitUsageRepository>()
            .AddScoped<IMonthlyChargeSnapshotRepository, MonthlyChargeSnapshotRepository>()
            .AddScoped<IProductConditionRepository, ProductConditionRepository>();
    }
}
