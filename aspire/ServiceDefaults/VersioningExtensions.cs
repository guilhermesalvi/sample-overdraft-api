using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceDefaults;

public static class VersioningExtensions
{
    public static void AddVersioning(this IServiceCollection services)
    {
        services
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            })
            .EnableApiVersionBinding();
    }
}
