using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ArrivalTimeAfterDepartureTimeAttribute : ValidationAttribute
{
    private readonly string _departureTimeProperty;

    public ArrivalTimeAfterDepartureTimeAttribute(string departureTimeProperty)
    {
        _departureTimeProperty = departureTimeProperty;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var departureTimeProperty = validationContext.ObjectType.GetProperty(_departureTimeProperty);

        if (departureTimeProperty == null)
        {
            throw new ArgumentException("Property with this name not found");
        }

        var departureTime = (DateTime)departureTimeProperty.GetValue(validationContext.ObjectInstance);
        var arrivalTime = (DateTime)value;

        if (arrivalTime <= departureTime)
        {
            return new ValidationResult(ErrorMessage ?? "Arrival time must be after departure time.");
        }

        return ValidationResult.Success;
    }
}