using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ServiceDefaults;

public static class HealthChecksResponseWriter
{
    public static async Task WriteJsonAsync(
        HttpContext httpContext,
        HealthReport report)
    {
        httpContext.Response.ContentType = "application/json; charset=utf-8";
        httpContext.Response.Headers.CacheControl = "no-store";

        var dto = new HealthCheckResponseDto(
            Status: report.Status.ToString(),
            TotalDuration: report.TotalDuration.ToString("c"),
            Entries: report.Entries.ToDictionary(
                kvp => kvp.Key,
                kvp => new HealthCheckEntryDto(
                    Name: kvp.Key,
                    Status: kvp.Value.Status.ToString(),
                    DurationMs: kvp.Value.Duration.TotalMilliseconds)));

        await httpContext.Response.WriteAsJsonAsync(
            dto,
            HealthChecksJsonSerializerContext.Default.HealthCheckResponseDto);
    }
}

public record HealthCheckEntryDto(
    string Name,
    string Status,
    double DurationMs);

public record HealthCheckResponseDto(
    string Status,
    string TotalDuration,
    Dictionary<string, HealthCheckEntryDto> Entries);

[JsonSerializable(typeof(HealthCheckResponseDto))]
[JsonSerializable(typeof(HealthCheckEntryDto))]
internal partial class HealthChecksJsonSerializerContext : JsonSerializerContext;
