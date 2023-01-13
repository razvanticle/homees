using Homees.Application.Common.Interfaces;
using Homees.Domain.Aggregates.Lights;
using MediatR;

namespace Homees.Application.Lights.Commands.CreateLight;

public record CreateLightCommand(string Name) : IRequest<Guid>;

public class CreateLightCommandHandler : IRequestHandler<CreateLightCommand, Guid>
{
    private readonly IEventStoreRepository<Light> eventStoreRepository;

    public CreateLightCommandHandler(IEventStoreRepository<Light> eventStoreRepository)
    {
        this.eventStoreRepository = eventStoreRepository;
    }
    
    public async Task<Guid> Handle(CreateLightCommand command, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();
        var light = new Light(id, command.Name);

        await eventStoreRepository.Add(light, cancellationToken);

        return light.Id;
    }
}