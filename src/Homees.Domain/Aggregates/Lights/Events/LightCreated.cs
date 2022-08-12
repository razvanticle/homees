using Homees.Domain.Common.Aggregates;
using Homees.Domain.Common.Events;

namespace Homees.Domain.Aggregates.Lights.Events;

public record LightCreated(Guid Id, string Name) : DomainEventBase
{
    public static LightCreated Create(Guid id, string name)
    {
        return new LightCreated(id, name);
    }
}