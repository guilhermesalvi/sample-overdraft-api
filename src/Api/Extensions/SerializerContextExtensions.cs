namespace Overdraft.Api.Extensions;

public static class SerializerContextExtensions
{
    public static void AddSerializerContext(this IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(options => { });
    }
}
