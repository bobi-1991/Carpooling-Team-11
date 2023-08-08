using System.ComponentModel.DataAnnotations;

namespace Carpooling.Models
{
    public class TravelViewModel
    {
        public string StartDestination { get; set; }
        public string EndDestination { get; set; }
        public string CityStartDest { get; set; }
        public string CityEndDest { get; set; }
        public string Country { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Available Seats must be a non-negative number.")]
        public int AvailableSeats { get; set; }
        public string CarRegistration { get; set; }
    }
}
