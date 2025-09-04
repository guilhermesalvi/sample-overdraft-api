using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Baggage = OpenTelemetry.Baggage;

namespace ServiceDefaults;

public static class ObservabilityExtensions
{
    public static void AddObservability<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        builder.Services.ConfigureHttpJsonOptions(options =>
            options.SerializerOptions.TypeInfoResolverChain.Insert(0, HealthChecksJsonSerializerContext.Default));

        builder.AddDefaultHealthChecks();
        builder.ConfigureOpenTelemetry();
    }

    public static void UseObservability(this WebApplication app)
    {
        app.MapDefaultHealthCheckEndpoints();
        app.UseTraceLoggingEnrichment();
    }

    private static void MapDefaultHealthCheckEndpoints(this WebApplication app)
    {
        app.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains("live"),
            ResponseWriter = HealthChecksResponseWriter.WriteJsonAsync
        });

        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains("ready"),
            ResponseWriter = HealthChecksResponseWriter.WriteJsonAsync
        });
    }

    private static void UseTraceLoggingEnrichment(this WebApplication app)
    {
        var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("Telemetry.LoggingContext");

        app.Use(async (_, next) =>
        {
            using (logger.BeginScope(new Dictionary<string, object?>
                   {
                       ["TraceId"] = Activity.Current?.TraceId.ToString() ?? string.Empty,
                       ["SpanId"] = Activity.Current?.SpanId.ToString() ?? string.Empty,
                       ["Baggage"] = string.Join(",", Baggage.Current.GetBaggage().Select(b => $"{b.Key}={b.Value}"))
                   }))
            {
                await next();
            }
        });
    }

    private static void AddDefaultHealthChecks<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        builder.Services
            .AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy(), tags: ["live"]);
    }

    private static void ConfigureOpenTelemetry<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        builder.Services
            .AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation();
            })
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.Filter = context =>
                            !context.Request.Path.StartsWithSegments("/health/live")
                            && !context.Request.Path.StartsWithSegments("/health/ready");
                    })
                    .AddHttpClientInstrumentation()
                    .AddSource(builder.Environment.ApplicationName);
            });
    }
}
