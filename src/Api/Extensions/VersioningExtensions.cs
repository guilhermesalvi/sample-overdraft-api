using Asp.Versioning;

namespace Overdraft.Api.Extensions;

public static class VersioningExtensions
{
    public static void AddVersioning(this IServiceCollection services)
    {
        services
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
            }).EnableApiVersionBinding();
    }
}
