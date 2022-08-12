using Homees.Domain.Common.Aggregates;
using Homees.Domain.Common.Events;

namespace Homees.Domain.Aggregates.Lights.Events;

public record LightTurnedOn(Guid Id) : DomainEventBase
{
    public static LightTurnedOn Create(Guid id)
    {
        return new LightTurnedOn(id);
    }
}