using FluentValidation;

namespace Homees.Application.Lights.Commands.CreateLight;

public class CreateLightCommandValidator : AbstractValidator<CreateLightCommand>
{
    public CreateLightCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
    }
}