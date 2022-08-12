using Homees.Domain.Common.Events;

namespace Homees.Domain.Common.Aggregates;

public abstract class Aggregate: Aggregate<Guid>, IAggregate
{
}

public abstract class Aggregate<T> : IAggregate<T> where T : notnull
{
    [NonSerialized] 
    private readonly Queue<DomainEventBase> uncommittedEvents = new();

    public T Id { get; protected set; } = default!;

    public int Version { get; protected set; }

    public abstract void When(DomainEventBase domainEvent);

    public DomainEventBase[] DequeueUncommittedEvents()
    {
        var dequeuedEvents = uncommittedEvents.ToArray();

        uncommittedEvents.Clear();

        return dequeuedEvents;
    }

    protected void Apply(DomainEventBase domainEvent)
    {
        When(domainEvent);
        uncommittedEvents.Enqueue(domainEvent);
    }
}