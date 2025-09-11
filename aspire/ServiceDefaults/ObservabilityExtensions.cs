using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Baggage = OpenTelemetry.Baggage;

namespace ServiceDefaults;

public static class ObservabilityExtensions
{
    private const string LivePath = "/health/live";
    private const string ReadyPath = "/health/ready";
    private const string BaggagePref = "baggage.";

    public static void AddObservability(this IHostApplicationBuilder builder)
    {
        builder.Services.ConfigureHttpJsonOptions(o =>
        {
            o.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            o.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            o.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

            o.SerializerOptions.TypeInfoResolverChain.Add(HealthChecksJsonSerializerContext.Default);
            o.SerializerOptions.TypeInfoResolverChain.Add(ProblemJsonSerializerContext.Default);
        });

        var settings = OtelSettings.From(builder);
        ConfigureHealthChecks(builder.Services);
        ConfigureOpenTelemetry(builder, settings);
        builder.Services.AddSingleton(_ => new ActivitySource(settings.ActivitySourceName));
    }

    public static void MapHealthEndpoints(this WebApplication app)
    {
        app.MapHealthChecks(LivePath, new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains("live"),
            ResponseWriter = HealthChecksResponseWriter.WriteJsonAsync
        });

        app.MapHealthChecks(ReadyPath, new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains("ready"),
            ResponseWriter = HealthChecksResponseWriter.WriteJsonAsync
        });
    }

    public static void UseBaggageEnrichment(this WebApplication app)
    {
        var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("Telemetry.BaggageScope");

        app.Use(async (_, next) =>
        {
            var bag = Baggage.Current.GetBaggage();
            if (bag.Count == 0)
            {
                await next();
                return;
            }

            var dict = new Dictionary<string, object?>(bag.Count);
            foreach (var kv in bag) dict[BaggagePref + kv.Key] = kv.Value;

            using (logger.BeginScope(dict)) await next();
        });
    }

    private static void ConfigureHealthChecks(IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy(), tags: ["live"])
            .AddCheck("startup", () => HealthCheckResult.Healthy(), tags: ["ready"]);
    }

    private static void ConfigureOpenTelemetry(IHostApplicationBuilder builder, OtelSettings s)
    {
        var sampler = new ParentBasedSampler(new TraceIdRatioBasedSampler(s.HeadSampleRatio));

        builder.Logging.ClearProviders();
        builder.Logging.AddOpenTelemetry(lo =>
        {
            lo.IncludeFormattedMessage = true;
            lo.IncludeScopes = true;
            lo.ParseStateValues = true;

            var rb = ResourceBuilder.CreateDefault();
            ConfigureResource(rb);
            lo.SetResourceBuilder(rb);

            lo.AddOtlpExporter();
        });

        builder.Services
            .AddOpenTelemetry()
            .ConfigureResource(ConfigureResource)
            .WithMetrics(mp =>
            {
                mp
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddMeter("System.Runtime")
                    .AddOtlpExporter();
            })
            .WithTracing(tp =>
            {
                tp
                    .AddAspNetCoreInstrumentation(o =>
                    {
                        o.RecordException = true;
                        o.Filter = ctx =>
                        {
                            var p = ctx.Request.Path;
                            return !p.StartsWithSegments("/health", StringComparison.OrdinalIgnoreCase)
                                   && !p.StartsWithSegments("/alive", StringComparison.OrdinalIgnoreCase)
                                   && !p.StartsWithSegments("/metrics", StringComparison.OrdinalIgnoreCase);
                        };
                        o.EnrichWithHttpRequest = (act, req) =>
                        {
                            act?.SetTag("http.client_ip", req.HttpContext.Connection.RemoteIpAddress?.ToString());
                            act?.SetTag("http.route.display_name", req.HttpContext.GetEndpoint()?.DisplayName);
                        };
                        o.EnrichWithHttpResponse = (act, res) =>
                        {
                            act?.SetTag("http.response_content_length", res.ContentLength);
                        };
                    })
                    .AddHttpClientInstrumentation(o =>
                    {
                        o.RecordException = true;
                        o.EnrichWithHttpRequestMessage = (act, req) =>
                            act?.SetTag("http.request.version", req.Version?.ToString());
                        o.EnrichWithHttpResponseMessage = (act, res) =>
                            act?.SetTag("http.response.version", res.Version?.ToString());
                    })
                    .AddSource(s.ActivitySourceName)
                    .SetSampler(sampler)
                    .AddOtlpExporter();
            });

        return;

        void ConfigureResource(ResourceBuilder rb)
        {
            // If aspire injects service.name, CreateDefault already contains it.
            // If you want to explicitly enforce service.name/version via contract:
            // rb.AddService(serviceName: s.ServiceName, serviceVersion: s.ServiceVersion, autoGenerateServiceInstanceId: true);

            rb.AddAttributes([new KeyValuePair<string, object>("deployment.environment", s.EnvironmentName)]);
        }
    }

    private sealed record OtelSettings(
        string ServiceName,
        string ServiceVersion,
        string EnvironmentName,
        double HeadSampleRatio,
        string ActivitySourceName)
    {
        public static OtelSettings From(IHostApplicationBuilder builder)
        {
            // OpenTelemetry settings via environment variables and conventions.
            // - OTEL_SERVICE_NAME: aspire injects this variable
            // - OTEL_SERVICE_VERSION: aspire injects this variable
            // - OTEL_HEAD_RATIO: defined by environment (e.g. 0.05 for 5% sampling)
            // - OTEL_EXPORTER_OTLP_ENDPOINT: apphost sets this to point to Aspire Collector

            var serviceName = Environment.GetEnvironmentVariable("OTEL_SERVICE_NAME")!;
            var serviceVer = Environment.GetEnvironmentVariable("OTEL_SERVICE_VERSION")!;
            var envName = builder.Environment.EnvironmentName;
            var headRatioStr = Environment.GetEnvironmentVariable("OTEL_HEAD_RATIO")!;
            var headRatio = double.Parse(headRatioStr, System.Globalization.CultureInfo.InvariantCulture);

            var sourceName = serviceName;

            return new OtelSettings(serviceName, serviceVer, envName, headRatio, sourceName);
        }
    }
}
