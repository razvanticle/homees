using Homees.Domain.Common.Aggregates;

namespace Homees.Application.Common.Interfaces;

public interface IEventStoreRepository<T>where T : class, IAggregate
{
    Task<T?> GetById(Guid id, CancellationToken cancellationToken);
    
    Task<ulong> Add(T aggregate, CancellationToken cancellationToken = default);

    Task<ulong> Update(T aggregate, ulong? expectedRevision = null, CancellationToken cancellationToken = default);
}