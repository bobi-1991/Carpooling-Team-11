using System.ComponentModel.DataAnnotations;

namespace Carpooling.Models
{
    public class TravelViewModel
    {
        [RegularExpression(@"^[A-Z][a-z]*$", ErrorMessage = "City name must start with a capital letter and be followed by lowercase letters.")]
        public string CityStartDest { get; set; }
        [RegularExpression(@"^[A-Z][a-z]*$", ErrorMessage = "City name must start with a capital letter and be followed by lowercase letters.")]
        public string CityEndDest { get; set; }
        [RegularExpression(@"^([A-Z][a-z]*(\s[A-Z][a-z]*)*)$", ErrorMessage = "Invalid country name format.")]
        public string Country { get; set; }
        [Display(Name = "Departure Time")]
        [DateTimeNotInPast]
        [DataType(DataType.DateTime)]
        public DateTime DepartureTime { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Available Seats must be a non-negative number.")]
        public int AvailableSeats { get; set; }
        [RegularExpression(@"^[A-ZА-Я0-9]+$", ErrorMessage = "Registration must contain only uppercase letters.")]
        public string CarRegistration { get; set; }
    }
}
