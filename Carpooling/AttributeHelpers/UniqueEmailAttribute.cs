using CarPooling.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Carpooling.AttributeHelpers
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success; // Allow null values (handled by Required attribute)
            }

            var userManager = validationContext.GetService<UserManager<User>>();
            if (userManager == null)
            {
                throw new InvalidOperationException("UserManager<IdentityUser> service not found.");
            }

            var existingUser = userManager.FindByEmailAsync(value.ToString()).Result;
            if (existingUser != null)
            {
                return new ValidationResult("Email is already registered.");
            }

            return ValidationResult.Success;
        }
    }
}
