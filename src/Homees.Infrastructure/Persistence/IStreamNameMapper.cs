using System.Collections.Concurrent;

namespace Homees.Infrastructure.Persistence;

public interface IStreamNameMapper
{
    string Map<TStream>(object aggregateId);
    
    string Map(Type streamType, object aggregateId);
}

internal class StreamNameMapper : IStreamNameMapper
{
    private static readonly ConcurrentDictionary<Type, string> TypeNameMap = new();

    public string Map<TStream>(object aggregateId) =>
        Map(typeof(TStream), aggregateId);
    
    public string Map(Type streamType, object aggregateId)
    {
        var streamPrefix = ToStreamPrefix(streamType);

        return $"{streamPrefix}-{aggregateId}";
    }
    
    private static string ToStreamPrefix(Type streamType) => TypeNameMap.GetOrAdd(streamType, _ =>
    {
        var modulePrefix = streamType.Namespace!.Split(".").First();
        return $"{modulePrefix}_{streamType.Name}";
    });
}
