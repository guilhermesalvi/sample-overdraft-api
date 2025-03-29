using System.Collections.Immutable;
using GSalvi.Toolkit.Notifications;

namespace Overdraft.Api.Endpoints;

public static class Responses
{
    public static IResult ToValidationProblem(this IImmutableSet<Notification> notifications)
    {
        return Results.ValidationProblem(notifications.Select(x =>
            new KeyValuePair<string, string[]>(x.Key, x.Value.Select(y => y.ToString()).ToArray())));
    }
}
