using Homees.Domain.Common.Aggregates;
using Homees.Domain.Common.Events;

namespace Homees.Domain.Aggregates.Lights.Events;

public record LightDisconnected(Guid Id) : DomainEventBase
{
    public static LightDisconnected Create(Guid id)
    {
        return new LightDisconnected(id);
    }
}