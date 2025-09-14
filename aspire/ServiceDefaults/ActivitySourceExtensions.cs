using System.Diagnostics;

namespace ServiceDefaults;

public static class ActivitySourceExtensions
{
    public static Activity? StartBusinessActivity(
        this ActivitySource source,
        string name,
        ActivityKind kind = ActivityKind.Internal,
        IEnumerable<KeyValuePair<string, object?>>? tags = null,
        ActivityContext parentContext = default) => source.StartActivity(name, kind, parentContext, tags);
}
