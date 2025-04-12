namespace Overdraft.Api.Extensions;

public static class DocumentationExtensions
{
    public static void AddDocumentation(this IServiceCollection services)
    {
        services.AddOpenApi();
    }

    public static void MapDocumentation(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
    }
}
