namespace CustomerEnrollment.CrossCutting.Diagnostics;

public static class EventNames
{
    public const string HandlerStart = "handler.start";
    public const string HandlerEnd = "handler.end";
    public const string QueryStart = "query.start";
    public const string QueryEnd = "query.end";
    public const string Error = "error";
    public const string ValidationStart = "validation.start";
    public const string ValidationEnd = "validation.end";
}
