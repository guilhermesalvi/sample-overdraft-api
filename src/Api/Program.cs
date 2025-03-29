using Overdraft.Api.Extensions;
using Overdraft.Application.Extensions;
using Overdraft.Infrastructure.Data.Extensions;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateSlimBuilder(args);

    // Add services to the container.
    builder.Services.AddVersioning();
    builder.Services.AddDocumentation();
    builder.Services.AddApplication(builder.Configuration);
    builder.Services.AddData(builder.Configuration);
    builder.Services.AddNotification();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.MapDocumentation();
    app.UseNotification();
    app.MapEndpoints();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}
