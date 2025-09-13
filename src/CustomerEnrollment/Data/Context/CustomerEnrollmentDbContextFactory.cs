using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CustomerEnrollment.Data.Context;

public sealed class CustomerEnrollmentDbContextFactory : IDesignTimeDbContextFactory<CustomerEnrollmentDbContext>
{
    public CustomerEnrollmentDbContext CreateDbContext(string[] args)
    {
        const string cs = "Server=localhost,1433;Database=CustomerEnrollment;TrustServerCertificate=True";

        var opts = new DbContextOptionsBuilder<CustomerEnrollmentDbContext>()
            .UseSqlServer(cs)
            .EnableSensitiveDataLogging()
            .Options;

        return new CustomerEnrollmentDbContext(opts);
    }
}
