namespace CustomerEnrollment.Extensions;

public static class DataExtensions
{
    public static void AddData(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("customer-enrollment-db")!;

        builder.Services
            .AddHealthChecks()
            .AddSqlServer(connectionString);
    }
}
