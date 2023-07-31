using Carpooling.BusinessLayer.Dto_s.Requests;
using FluentValidation;

namespace Carpooling.Fluent_Validation
{
    public class AddressRequestValidator : AbstractValidator<AddressDTO>
    {
        public AddressRequestValidator()
        {
            RuleFor(x => x.Country)
                .NotEmpty()
                .WithMessage("Property is required");

            RuleFor(x => x.Details)
                .NotEmpty()
                .WithMessage("Property is required");

            RuleFor(x => x.City)
                .NotEmpty()
                .WithMessage("Property is required");
        }
    }
}
