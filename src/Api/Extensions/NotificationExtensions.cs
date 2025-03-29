using System.Globalization;
using GSalvi.Toolkit.Notifications;
using Microsoft.AspNetCore.Localization;
using Overdraft.Api.Resources;

namespace Overdraft.Api.Extensions;

public static class NotificationExtensions
{
    public static void AddNotification(this IServiceCollection services)
    {
        services.AddLocalizedNotifications<SharedResource>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture("pt-BR");
            options.SupportedCultures = [new CultureInfo("pt-BR"), new CultureInfo("en-US")];
        });
    }

    public static void UseNotification(this WebApplication app)
    {
        app.UseLocalizedNotifications();
    }
}
