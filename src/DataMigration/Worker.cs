using System.Diagnostics;
using CustomerEnrollment.CrossCutting.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace DataMigration;

public class Worker(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource ActivitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        using var activity = ActivitySource.StartActivity(ActivityKind.Client);

        try
        {
            await RunMigrationAsync(ct);
        }
        catch (Exception ex)
        {
            activity?.AddException(ex);
            throw;
        }

        hostApplicationLifetime.StopApplication();
    }

    private async Task RunMigrationAsync(CancellationToken ct)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var firstContext = scope.ServiceProvider.GetRequiredService<CustomerEnrollmentDbContext>();

        await firstContext.Database.MigrateAsync(cancellationToken: ct);
    }
}
