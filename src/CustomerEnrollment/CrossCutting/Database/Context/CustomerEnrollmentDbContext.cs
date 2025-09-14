using CustomerEnrollment.OverdraftAccounts;
using CustomerEnrollment.OverdraftAccounts.Aggregates;
using CustomerEnrollment.OverdraftContracts;
using Microsoft.EntityFrameworkCore;

namespace CustomerEnrollment.CrossCutting.Database.Context;

public class CustomerEnrollmentDbContext(DbContextOptions<CustomerEnrollmentDbContext> options) : DbContext(options)
{
    public DbSet<OverdraftAccount> OverdraftAccounts => Set<OverdraftAccount>();
    public DbSet<OverdraftContract> OverdraftContracts => Set<OverdraftContract>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerEnrollmentDbContext).Assembly);
    }
}
