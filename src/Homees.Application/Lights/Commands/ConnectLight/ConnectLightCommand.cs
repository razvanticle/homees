using Homees.Application.Common.Exceptions;
using Homees.Application.Common.Interfaces;
using Homees.Domain.Aggregates.Lights;
using MediatR;

namespace Homees.Application.Lights.Commands.ConnectLight;

public record ConnectLightCommand(Guid Id) : IRequest<Unit>;

public class ConnectLightCommandHandler : IRequestHandler<ConnectLightCommand, Unit>
{
    private readonly IEventStoreRepository<Light> eventStoreRepository;

    public ConnectLightCommandHandler(IEventStoreRepository<Light> eventStoreRepository)
    {
        this.eventStoreRepository = eventStoreRepository;
    }
    
    public async Task<Unit> Handle(ConnectLightCommand request, CancellationToken cancellationToken)
    {
        var light = await eventStoreRepository.GetById(request.Id, cancellationToken);
        if (light == null)
        {
            throw new EntityNotFoundException(nameof(Light), request.Id);
        }
        
        light.Connect();
        await eventStoreRepository.Update(light, null, cancellationToken);

        return Unit.Value;
    }
}