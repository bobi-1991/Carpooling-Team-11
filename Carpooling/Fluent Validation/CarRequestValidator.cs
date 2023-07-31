using Carpooling.BusinessLayer.Dto_s.Requests;
using FluentValidation;

namespace Carpooling.Fluent_Validation
{
    public class CarRequestValidator : AbstractValidator<CarDTO>
    {
        public CarRequestValidator()
        {
            RuleFor(x => x.Color)
                .NotEmpty()
                .WithMessage("Property cannot be empty");

            RuleFor(x => x.Model)
                .NotEmpty()
                .WithMessage("Property cannot be empty");

            RuleFor(x => x.Brand)
                .NotEmpty()
                .WithMessage("Property cannot be empty");

            RuleFor(x => x.AvailableSeats)
                .NotEmpty()
                .WithMessage("Property cannot be empty")
                .GreaterThan(-1)
                .LessThan(5);

            RuleFor(x => x.TotalSeats)
                .NotEmpty()
                .WithMessage("Property cannot be empty")
                .GreaterThan(0)
                .LessThan(5);

            RuleFor(x => x.Registration)
                .NotEmpty()
                .WithMessage("Property cannot be empty");
        }
    }
}
