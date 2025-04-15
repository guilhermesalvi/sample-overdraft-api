using Serilog.Core;
using Serilog.Core.Enrichers;

namespace Overdraft.Application.Logging;

public class LogEnricherBuilder
{
    private readonly HashSet<ILogEventEnricher> _enrichers = [];

    public LogEnricherBuilder WithProperty(string name, string value)
    {
        _enrichers.Add(new PropertyEnricher(name, value));
        return this;
    }

    public ILogEventEnricher[] Build() => _enrichers.ToArray();

    public static implicit operator ILogEventEnricher[](LogEnricherBuilder builder) => builder.Build();
}
