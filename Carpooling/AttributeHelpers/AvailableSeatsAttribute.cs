using System;
using System.ComponentModel.DataAnnotations;

namespace Carpooling.AttributeHelpers
{
    public class AvailableSeatsAttribute : ValidationAttribute
    {
        private readonly string _totalSeatsPropertyName;

        public AvailableSeatsAttribute(string totalSeatsPropertyName)
        {
            _totalSeatsPropertyName = totalSeatsPropertyName;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var totalSeatsProperty = validationContext.ObjectType.GetProperty(_totalSeatsPropertyName);

            if (totalSeatsProperty == null)
            {
                throw new ArgumentException($"Property {_totalSeatsPropertyName} not found on {validationContext.ObjectType}");
            }

            var totalSeatsValue = (int)totalSeatsProperty.GetValue(validationContext.ObjectInstance);
            var availableSeatsValue = (int)value;

            if (availableSeatsValue > totalSeatsValue)
            {
                return new ValidationResult($"Available seats cannot be greater than total seats ({totalSeatsValue}).");
            }

            return ValidationResult.Success;
        }
    }
}
