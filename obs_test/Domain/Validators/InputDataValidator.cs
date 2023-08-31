using FluentValidation;
using obs_test.Domain.Models;

namespace obs_test.Domain.Validators
{
    public class InputDataValidator : AbstractValidator<InputData>
    {
        public InputDataValidator()
        {
            RuleFor(input => input.Terrain).SetValidator(new TerrainValidator());
            RuleFor(input => input.Battery).GreaterThan(0).WithMessage("The robot has not remaining  battery");
            RuleFor(input => input.Commands).NotEmpty().WithMessage("No commands provided.");
            RuleForEach(input => input.Commands).Must(command => CommandParser.TryParse(command, out _)).WithMessage("Invalid command: {PropertyValue}");
        }
    }
}
