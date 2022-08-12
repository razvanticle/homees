using Homees.Domain.Common.Events;

namespace Homees.Domain.Common.Aggregates;

public interface IAggregate: IAggregate<Guid>
{
}

public interface IAggregate<out T> : IProjection
{
    T Id { get; }
    int Version { get; }

    DomainEventBase[] DequeueUncommittedEvents();
}