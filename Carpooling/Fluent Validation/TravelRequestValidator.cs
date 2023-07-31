using Carpooling.Service.Dto_s.Requests;
using FluentValidation;

namespace Carpooling.Fluent_Validation
{
    public class TravelRequestValidator : AbstractValidator<TravelRequest>
    {
        public TravelRequestValidator()
        {
            RuleFor(x => x.CarId)
                .NotEmpty()
                .WithMessage("Required Id")
                .GreaterThan(0)
                .WithMessage("Cannot be negative");

            RuleFor(x => x.StartLocationId)
                .NotEmpty()
                .WithMessage("Required Id")
                .GreaterThan(0)
                .WithMessage("Cannot be negative");

            RuleFor(x => x.DestionationId)
                .NotEmpty()
                .WithMessage("Required Id")
                .GreaterThan(0)
                .WithMessage("Cannot be negative");

            RuleFor(x => x.DriverId)
                .NotEmpty()
                .WithMessage("Required Id");
        }
    }
}
