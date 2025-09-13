using Microsoft.EntityFrameworkCore;

namespace CustomerEnrollment.Data.Context;

public class CustomerEnrollmentDbContext(DbContextOptions<CustomerEnrollmentDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerEnrollmentDbContext).Assembly);
    }
}
