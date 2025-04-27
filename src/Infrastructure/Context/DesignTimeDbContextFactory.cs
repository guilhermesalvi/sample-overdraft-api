using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Context;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

#pragma warning disable S2068 // Design-time only
        const string connStr =
            "Server=localhost,1433;Database=OverdraftConcession;User Id=sa;Password=fijnif-kuVxit-jivma4;TrustServerCertificate=True;";
#pragma warning restore S2068

        builder.UseSqlServer(
            connStr,
            sql => sql.MigrationsAssembly(typeof(DesignTimeDbContextFactory).Assembly.FullName));

        return new ApplicationDbContext(builder.Options);
    }
}
