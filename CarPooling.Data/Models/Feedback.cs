using CarPooling.Data.Models.Abstract;
using System.Text.Json.Serialization;

namespace CarPooling.Data.Models
{
    public class Feedback : EntityBase
    {
        public int? Rating { get; set; }

        public string? Comment { get; set; }

        // Foreign key for Passenger
        [JsonIgnore]
        public string GiverId { get; set; }

        // Navigation property for Passenger
        [JsonIgnore]
        public User Giver { get; set; }

        // Foreign key for Driver
        [JsonIgnore]
        public string ReceiverId { get; set; }

        // Navigation property for Driver
        [JsonIgnore]
        public User Receiver { get; set; }

        // Navigation property for TravelId
        [JsonIgnore]
        public int TravelId { get; set; }

        // Navigation property for Travel
        [JsonIgnore]
        public Travel Travel { get; set; }
    }
}
