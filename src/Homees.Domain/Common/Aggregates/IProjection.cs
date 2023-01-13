using Homees.Domain.Common.Events;

namespace Homees.Domain.Common.Aggregates;

public interface IProjection
{
    void When(object domainEvent);
}