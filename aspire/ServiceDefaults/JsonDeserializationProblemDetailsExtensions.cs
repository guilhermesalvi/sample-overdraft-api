using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServiceDefaults;

public static class JsonDeserializationProblemDetailsExtensions
{
    public static void UseJsonDeserializationProblemDetails(this WebApplication app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async ctx =>
            {
                if (ctx.Response.HasStarted) return;

                var error = ctx.Features.Get<IExceptionHandlerFeature>()?.Error;

                switch (error)
                {
                    case JsonException je:
                        await WriteJsonValidation(ctx, je.Path, "Invalid payload or incompatible data type.");
                        return;
                    case BadHttpRequestException { InnerException: JsonException innerJe }:
                        await WriteJsonValidation(ctx, innerJe.Path, "Invalid payload or incompatible data type.");
                        return;
                }
            });
        });
    }

    private static async Task WriteJsonValidation(HttpContext ctx, string? jsonPath, string message)
    {
        var key = ExtractField(jsonPath) ?? "$";
        var errors = new Dictionary<string, string[]> { [key] = [message] };

        var problem = new HttpValidationProblemDetails(errors)
        {
            Title = "Invalid request payload",
            Status = StatusCodes.Status400BadRequest
        };

        ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
        ctx.Response.ContentType = "application/problem+json";
        ctx.Response.Headers.CacheControl = "no-store";

        await ctx.Response.WriteAsJsonAsync(problem, ProblemJsonSerializerContext.Default.HttpValidationProblemDetails);
    }

    private static string? ExtractField(string? path)
    {
        if (string.IsNullOrWhiteSpace(path)) return null;
        var last = path.LastIndexOf('.');
        var seg = last >= 0 ? path[(last + 1)..] : path;
        var bracket = seg.IndexOf('[');
        if (bracket >= 0) seg = seg[..bracket];
        if (seg.StartsWith('$')) seg = seg.TrimStart('$').TrimStart('.');
        return string.IsNullOrWhiteSpace(seg) ? null : seg;
    }
}

[JsonSerializable(typeof(ProblemDetails))]
[JsonSerializable(typeof(HttpValidationProblemDetails))]
internal partial class ProblemJsonSerializerContext : JsonSerializerContext;
