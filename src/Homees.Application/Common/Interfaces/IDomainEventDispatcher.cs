using Homees.Domain.Common.Events;

namespace Homees.Application.Common.Interfaces;

public interface IDomainEventDispatcher
{
    Task Dispatch(IReadOnlyCollection<DomainEventBase> events);
}