using System.Data;
using Microsoft.Data.SqlClient;

namespace ClientEnrollment.Extensions;

public static class DataExtensions
{
    public static void AddData(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("customer-enrollment-db")!;

        builder.Services
            .AddHealthChecks()
            .AddSqlServer(connectionString);

        builder.Services
            .AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));
    }
}
