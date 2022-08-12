using Homees.Domain.Aggregates.Lights;
using MediatR;

namespace Homees.Application.Lights.Commands.CreateLight;

public record CreateLightCommand(string Name):IRequest<Guid>;

public class CreateLightCommandHandler : IRequestHandler<CreateLightCommand, Guid>
{
    public Task<Guid> Handle(CreateLightCommand command, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();
        
        var light = new Light(id, command.Name);

        return Task.FromResult(light.Id);
    }
}