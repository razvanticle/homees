using Homees.Application.Common.Exceptions;
using Homees.Application.Common.Interfaces;
using Homees.Domain.Aggregates.Lights;
using MediatR;

namespace Homees.Application.Lights.Commands.TurnOnLight;

public record TurnOnLightCommand(Guid Id) : IRequest<Unit>;

public class TurnOnLightCommandHandler : IRequestHandler<TurnOnLightCommand, Unit>
{
    private readonly IEventStoreRepository<Light> eventStoreRepository;

    public TurnOnLightCommandHandler(IEventStoreRepository<Light> eventStoreRepository)
    {
        this.eventStoreRepository = eventStoreRepository;
    }
    
    public async Task<Unit> Handle(TurnOnLightCommand request, CancellationToken cancellationToken)
    {
        var light = await eventStoreRepository.GetById(request.Id, cancellationToken);
        if (light == null)
        {
            throw new EntityNotFoundException(nameof(Light), request.Id);
        }
        
        light.TurnOn();
        await eventStoreRepository.Update(light, null, cancellationToken);

        return Unit.Value;
    }
}