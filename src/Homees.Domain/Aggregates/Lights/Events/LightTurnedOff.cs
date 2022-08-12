using Homees.Domain.Common.Aggregates;
using Homees.Domain.Common.Events;

namespace Homees.Domain.Aggregates.Lights.Events;

public record LightTurnedOff(Guid Id) : DomainEventBase
{
    public static LightTurnedOff Create(Guid id)
    {
        return new LightTurnedOff(id);
    }
}