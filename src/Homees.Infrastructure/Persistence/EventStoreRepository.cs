using EventStore.Client;
using Homees.Application.Common.Interfaces;
using Homees.Domain.Common.Aggregates;

namespace Homees.Infrastructure.Persistence;

public class EventStoreRepository<T> : IEventStoreRepository<T> where T : class, IAggregate
{
    private readonly EventStoreClient eventStore;
    private readonly IStreamNameMapper streamNameMapper;
    private readonly IEventStoreSerializer serializer;

    public EventStoreRepository(EventStoreClient eventStore,IStreamNameMapper streamNameMapper, IEventStoreSerializer serializer)
    {
        this.eventStore = eventStore;
        this.streamNameMapper = streamNameMapper;
        this.serializer = serializer;
    }
    
    public async Task<ulong> Add(T aggregate, CancellationToken cancellationToken = default)
    {
        var eventsToStore = GetEventsToStore(aggregate);
        
        var result = await eventStore.AppendToStreamAsync(
            streamNameMapper.Map<T>(aggregate.Id),
            StreamState.NoStream,
            eventsToStore,
            cancellationToken: cancellationToken
        );
        
        return result.NextExpectedStreamRevision;
    }
    
    public async Task<ulong> Update(T aggregate, ulong? expectedRevision = null, CancellationToken cancellationToken = default)
    {
        var eventsToAppend = GetEventsToStore(aggregate);
        
        // todo add concurrency tests https://developers.eventstore.com/clients/dotnet/5.0/embedded.html#building-a-node
        var revision = expectedRevision ?? (ulong)(aggregate.Version - eventsToAppend.Count);

        var result = await eventStore.AppendToStreamAsync(
            streamNameMapper.Map<T>(aggregate.Id),
            revision,
            eventsToAppend,
            cancellationToken: cancellationToken
        );
        return result.NextExpectedStreamRevision;
    }

    public Task<T?> GetById(Guid id, CancellationToken cancellationToken)
    {
        var aggregate = AggregateStream(id, cancellationToken);

        return aggregate;
    }
    
    private async Task<T?> AggregateStream(
        Guid id,
        CancellationToken cancellationToken,
        ulong? fromVersion = null
    ) 
    {
        var streamName = streamNameMapper.Map<T>(id);
        
        var readResult = eventStore.ReadStreamAsync(
            Direction.Forwards,
            streamName,
            fromVersion ?? StreamPosition.Start,
            cancellationToken: cancellationToken
        );

        if (await readResult.ReadState == ReadState.StreamNotFound)
            return null;

        var aggregate = (T)Activator.CreateInstance(typeof(T), true)!;

        await foreach (var @event in readResult)
        {
            var eventData = serializer.Deserialize(@event);

            aggregate.When(eventData!);
        }

        return aggregate;
    }

    private List<EventData> GetEventsToStore(T aggregate)
    {
        var events = aggregate.DequeueUncommittedEvents();

        return events
            .Select(x => ToJsonEventData(x))
            .ToList();
    }

    private EventData ToJsonEventData(object @event, object? metadata = null)
    {
        var serializedData = serializer.Serialize(@event);
        var serializedMetadata = serializer.Serialize(metadata ?? new { });
        
        return new EventData(
            Uuid.NewUuid(),
            @event.GetTypeName(),
            serializedData,
            serializedMetadata
        );
    }
}