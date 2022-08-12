using Homees.Domain.Common.Aggregates;
using Homees.Domain.Common.Events;

namespace Homees.Domain.Aggregates.Lights.Events;

public record LightConnected(Guid Id) : DomainEventBase
{
    public static LightConnected Create(Guid id)
    {
        return new LightConnected(id);
    }
}