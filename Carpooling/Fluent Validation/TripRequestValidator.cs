using Carpooling.Service.Dto_s.Requests;
using FluentValidation;

namespace Carpooling.Fluent_Validation
{
    public class TripRequestValidator : AbstractValidator<TripRequestRequest>
    {
        public TripRequestValidator()
        {
            RuleFor(x => x.TravelId)
                .NotEmpty()
                .WithMessage("Required Id")
                .GreaterThan(0)
                .WithMessage("Cannot be negative");

            RuleFor(x => x.PassengerId)
                .NotEmpty()
                .WithMessage("Required Id");

        }
    }
}
