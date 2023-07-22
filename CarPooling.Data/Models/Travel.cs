using CarPooling.Data.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Models
{
    public class Travel : Entity
    {
        public Travel()
        {
        }

        public Travel(int startLocationId, int destinationLocationId, DateTime departureTime, int availableSeats, int carId)
            : base()
        {
            this.DepartureTime = departureTime;
            this.AvailableSeat = availableSeats;
            this.StartLocationId = startLocationId;
            this.DestinationId = destinationLocationId;
            this.CarId = carId;
        }
        public DateTime DepartureTime { get; set; }
        public int AvailableSeat { get; set; }


        // Foreign keys with navigation properties
        public int StartLocationId { get; set; }
        public Address StartLocation { get; set; }

        public int DestinationId { get; set; }
        public Address Destination { get; set; }

        public int CarId { get; set; }
        public Car Car { get; set; }


        public ICollection<User> Passengers { get; set; } = new List<User>();
    }
}
