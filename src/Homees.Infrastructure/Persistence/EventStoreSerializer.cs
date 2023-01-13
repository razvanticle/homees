using System.Text;
using System.Text.Json;
using EventStore.Client;

namespace Homees.Infrastructure.Persistence;

public class EventStoreSerializer : IEventStoreSerializer
{
    private static object? Deserialize(ReadOnlySpan<byte> data, string eventType)
    {
        var decodedData = Encoding.UTF8.GetString(data);
        
        var @event = JsonSerializer.Deserialize(decodedData, AppDomain.CurrentDomain.GetTypeByName(eventType)!);

        return @event;
    }
    
    public object? Deserialize(ResolvedEvent resolvedEvent)
    {
        var data = resolvedEvent.Event.Data.Span;
        var decodedData = Encoding.UTF8.GetString(data);

        var @event = JsonSerializer.Deserialize(decodedData,
            AppDomain.CurrentDomain.GetTypeByName(resolvedEvent.Event.EventType)!);

        return @event;
    }

    public byte[] Serialize(object data)
    {
        var serializedData = JsonSerializer.Serialize(data);
        
        return Encoding.UTF8.GetBytes(serializedData);
    }
}