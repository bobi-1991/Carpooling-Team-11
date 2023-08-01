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
                .GreaterThan(-1)
                .WithMessage("Seats cannot be negative")
                .LessThan(5)
                .WithMessage("Seats cannot be more than 4");

            RuleFor(x => x.TotalSeats)
                .NotEmpty()
                .WithMessage("Property cannot be empty")
                .GreaterThan(0)
                .WithMessage("Seats cannot be negative")
                .LessThan(6)
                .WithMessage("Seats cannot be more than 5");

            RuleFor(x => x.Registration)
                .NotEmpty()
                .WithMessage("Property cannot be empty");
        }
    }
}
