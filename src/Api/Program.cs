using Overdraft.Api.Extensions;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateSlimBuilder(args);

    // Add services to the container.
    builder.Services.AddGlobalExceptionHandler();
    builder.Services.AddVersioning();
    builder.Services.AddDocumentation();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseExceptionHandler();
    app.MapDocumentation();
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
