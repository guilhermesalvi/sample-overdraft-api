using CustomerEnrollment.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CustomerEnrollment.Extensions;

public static class DataExtensions
{
    public static void AddData(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("customer-enrollment-db")
                               ?? throw new InvalidOperationException(
                                   "Connection string 'customer-enrollment-db' not found.");

        builder.Services.AddDbContext<CustomerEnrollmentDbContext>(options =>
        {
            options
                .UseSqlServer(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
        });

        builder.Services.AddHealthChecks().AddDbContextCheck<CustomerEnrollmentDbContext>();
    }
}
