using CarPooling.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Carpooling.AttributeHelpers
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class UniquePhoneNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var userManager = (UserManager<User>)validationContext.GetService(typeof(UserManager<User>));
            var phoneNumber = value as string;

            if (userManager.Users.Any(u => u.PhoneNumber == phoneNumber))
            {
                return new ValidationResult("Phone number is already registered.");
            }

            return ValidationResult.Success;
        }
    }
}
