using Carpooling.BusinessLayer.Dto_s.Requests;
using FluentValidation;

namespace Carpooling.Fluent_Validation
{
    public class CountryRequestValidator : AbstractValidator<CountryDTO>
    {
        public CountryRequestValidator()
        {
            RuleFor(x => x.Country)
                .NotEmpty()
                .WithMessage("Country cannot be empty");
        }
    }
}
