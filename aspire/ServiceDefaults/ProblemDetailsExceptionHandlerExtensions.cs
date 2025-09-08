using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceDefaults;

public static class ProblemDetailsExceptionHandlerExtensions
{
    public static void UseJsonBodyProblemDetails(this WebApplication app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async ctx =>
            {
                var error = ctx.Features.Get<IExceptionHandlerFeature>()?.Error;

                switch (error)
                {
                    case JsonException je:
                        await WriteJsonValidation(ctx, je.Path, "Invalid payload or incompatible data type.");
                        return;
                    case BadHttpRequestException { InnerException: JsonException innerJe }:
                        await WriteJsonValidation(ctx, innerJe.Path, "Invalid payload or incompatible data type.");
                        return;
                    case BadHttpRequestException bhe:
                    {
                        var status = bhe.StatusCode;

                        if (status == StatusCodes.Status400BadRequest)
                        {
                            var msg = NormalizeBadRequestMessage(bhe.Message);
                            await WriteJsonValidation(ctx, jsonPath: null, message: msg);
                            return;
                        }

                        await WriteProblem(ctx, status, title: StatusTitle(status));
                        return;
                    }
                    default:
                        await WriteProblem(ctx, StatusCodes.Status500InternalServerError, "Unexpected error");
                        break;
                }
            });
        });
    }

    private static async Task WriteJsonValidation(HttpContext ctx, string? jsonPath, string message)
    {
        var errors = new Dictionary<string, string[]> {[ExtractField(jsonPath) ?? "$"] = [message] };

        ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
        ctx.Response.ContentType = "application/problem+json";
        var problem = new HttpValidationProblemDetails(errors)
        {
            Title = "Invalid request payload",
            Status = StatusCodes.Status400BadRequest
        };

        await ctx.Response.WriteAsJsonAsync(problem, ProblemJsonSerializerContext.Default.HttpValidationProblemDetails);
    }

    private static async Task WriteProblem(HttpContext ctx, int status, string title)
    {
        ctx.Response.StatusCode = status;
        ctx.Response.ContentType = "application/problem+json";
        var problem = new ProblemDetails { Title = title, Status = status };
        await ctx.Response.WriteAsJsonAsync(problem, ProblemJsonSerializerContext.Default.ProblemDetails);
    }

    private static string StatusTitle(int status) => status switch
    {
        StatusCodes.Status413PayloadTooLarge => "Payload too large",
        StatusCodes.Status415UnsupportedMediaType => "Unsupported media type",
        StatusCodes.Status400BadRequest => "Bad request",
        _ => "Request error"
    };

    private static string NormalizeBadRequestMessage(string raw)
    {
        if (raw.Contains("Required body", StringComparison.OrdinalIgnoreCase))
            return "Request body is required.";
        if (raw.Contains("too large", StringComparison.OrdinalIgnoreCase))
            return "Payload too large.";
        if (raw.Contains("Content-Type", StringComparison.OrdinalIgnoreCase))
            return "Unsupported or incorrect Content-Type.";
        return "Invalid request body.";
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

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(HttpValidationProblemDetails))]
[JsonSerializable(typeof(ProblemDetails))]
internal partial class ProblemJsonSerializerContext : JsonSerializerContext;
