using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Overdraft.Application.Extensions;

public static class ApplicationExtensions
{
    public static void AddApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ApplicationExtensions).Assembly);
        });
    }
}
