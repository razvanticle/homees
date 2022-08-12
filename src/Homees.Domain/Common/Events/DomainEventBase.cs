namespace Homees.Domain.Common.Events;

public abstract record DomainEventBase
{
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}