using EventStore.Client;

namespace Homees.Infrastructure.Persistence;

public interface IEventStoreSerializer
{
    public byte[] Serialize(object data);
    
    object? Deserialize(ResolvedEvent resolvedEvent);
}