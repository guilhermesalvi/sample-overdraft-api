using System.Data;
using Microsoft.Data.SqlClient;
using Overdraft.Api.Data;

namespace Overdraft.Api.Extensions;

public static class DataExtensions
{
    public static void AddData(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IDbConnection>(_ =>
            new SqlConnection(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<AccountRepository>();
        services.AddScoped<ContractRepository>();
        services.AddScoped<DailyLimitRepository>();
    }
}
