using Homees.Domain.Common.Aggregates;
using Homees.Domain.Common.Events;

namespace Homees.Domain.Aggregates.Lights.Events;

public record LightDimmerValueUpdated(Guid Id, byte Value) : DomainEventBase
{
    public static LightDimmerValueUpdated Create(Guid id, byte value)
    {
        return new LightDimmerValueUpdated(id, value);
    }
}