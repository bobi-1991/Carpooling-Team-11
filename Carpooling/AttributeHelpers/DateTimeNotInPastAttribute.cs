using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class DateTimeNotInPastAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTime dateTime && dateTime < DateTime.Now)
        {
            return new ValidationResult("Departure time cannot be in the past.");
        }

        return ValidationResult.Success;
    }
}