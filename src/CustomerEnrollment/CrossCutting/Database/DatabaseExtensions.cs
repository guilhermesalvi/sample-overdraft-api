using CustomerEnrollment.CrossCutting.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace CustomerEnrollment.CrossCutting.Database;

public static class DatabaseExtensions
{
    public static void AddDatabase(this WebApplicationBuilder builder)
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
