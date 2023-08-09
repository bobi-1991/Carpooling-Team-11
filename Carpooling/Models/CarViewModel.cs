using System.ComponentModel.DataAnnotations;
using Carpooling.AttributeHelpers;
namespace Carpooling.Models
{
    public class CarViewModel
    {
        [RegularExpression(@"^[A-ZА-Я0-9]+$", ErrorMessage = "Registration must contain only uppercase letters.")]
        public string Registration { get; set; }
        public int TotalSeats { get; set; }
        [AvailableSeatsAttribute("TotalSeats", ErrorMessage = "Available seats cannot be greater than total seats.")]
        public int AvailableSeats { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        [RegularExpression(@"^[A-Z][a-z]*$", ErrorMessage = "Color must start with a capital letter and be followed by lowercase letters.")]
        public string Color { get; set; }
        public bool CanSmoke { get; set; }
    }
}
