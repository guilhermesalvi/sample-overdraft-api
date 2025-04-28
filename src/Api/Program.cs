using Api.Extensions;
using Application.Extensions;
using Infrastructure.Extensions;
using Serilog;
using Serilog.Core;
using Serilog.Events;

var levelSwitch = new LoggingLevelSwitch(LogEventLevel.Debug);
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.ControlledBy(levelSwitch)
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => lc
        .ReadFrom.Configuration(ctx.Configuration));

    // Add services to the container.
    builder.Services.AddHeaderPropagation(options => options.Headers.Add("X-Correlation-ID"));
    builder.Services.AddGlobalExceptionHandler();
    builder.AddEndpoints();
    builder.Services.AddVersioning();
    builder.Services.AddDocumentation();
    builder.Services.AddApplication();
    builder.Services.AddData(builder.Configuration);

    builder.Host.UseDefaultServiceProvider(options =>
    {
        options.ValidateOnBuild = true;
        options.ValidateScopes = true;
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseExceptionHandler();
    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.UseHeaderPropagation();
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
