using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Overdraft.Domain.Models;

namespace Overdraft.Infrastructure.Contexts;

[method: DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(ApplicationDbContext))]
public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<ContractAgreement> ContractAgreements { get; set; }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<DailyLimitUsageEntry> DailyLimitUsageEntries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
