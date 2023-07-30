using CarPooling.Data.Models.Abstract;
using System.Text.Json.Serialization;

namespace CarPooling.Data.Models
{
    public class Feedback : EntityBase
    {
        public int? Rating { get; set; }

        public string? Comment { get; set; }

        // Foreign key for Passenger
        public string PassengerId { get; set; }

        // Navigation property for Passenger
        public User Passenger { get; set; }

        // Foreign key for Driver
        public string DriverId { get; set; }

        // Navigation property for Driver
        
        public User Driver { get; set; }

        // Navigation property for TravelId
        public int TravelId { get; set; }

        // Navigation property for Travel
        public Travel Travel { get; set; }
    }
}
