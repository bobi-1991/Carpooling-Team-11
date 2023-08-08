using CarPooling.Data.Models.Abstract;
using System.Text.Json.Serialization;

namespace CarPooling.Data.Models
{
    public class Car : EntityBase
    {
        //public Car()
        //{
        //}

        public string Registration { get; set; }
        public int TotalSeats { get; set; } = 4;
        public int AvailableSeats { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public bool CanSmoke { get; set; }

        // Foreign keys with navigation properties
        //[JsonIgnore]
        public string DriverId { get; set; }
        //[JsonIgnore]
        public User Driver { get; set; }
    }
}
