using Carpooling.Service.Dto_s.Requests;
using FluentValidation;

namespace Carpooling.BusinessLayer.Validation.Fluent_Validation
{
    public class UserRequestValidator : AbstractValidator<UserRequest>
    {
        private const int MinNameLength = 2;
        private const int MaxNameLength = 20;
        private const int MinPasswordLength = 8;

        private const string RequiredErrorMessage = "{PropertyName} is required.";
        private const string LengthErrorMessage =
            "{PropertyName} must be between {MinLength} and {MaxLength} characters long. You entered {TotalLength} characters";
        private const string LengthMinPasswordMessage =
            "{PropertyName} must be minimum {MinLength}";

        private const string PasswordRegex = @"^(?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[A-Z])(?=.*[a-z]).*$";
        private const string PasswordRegexErrorMessage = "{PropertyName} must contain at least one uppercase letter, one lowercase letter and one digit. You entered {PropertyValue}";

        private const string InvalidErrorMessage = "{PropertyName} is invalid. You entered {PropertyValue}";

        public UserRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage(RequiredErrorMessage)
                .Length(MinNameLength, MaxNameLength)
                .WithMessage(LengthErrorMessage);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage(RequiredErrorMessage)
                .Length(MinNameLength, MaxNameLength)
                .WithMessage(LengthErrorMessage);

            RuleFor(x => x.Username)
              .NotEmpty()
              .WithMessage(RequiredErrorMessage)
              .Length(MinNameLength, MaxNameLength)
              .WithMessage(LengthErrorMessage);

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(RequiredErrorMessage)
                .MinimumLength(MinPasswordLength)
                .WithMessage(LengthMinPasswordMessage)
                .Matches(PasswordRegex)
                .WithMessage(PasswordRegexErrorMessage);

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(RequiredErrorMessage)
                .EmailAddress()
                .WithMessage(InvalidErrorMessage);
        }
    }
}
