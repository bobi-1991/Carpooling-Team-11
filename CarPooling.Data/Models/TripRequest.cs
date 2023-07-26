using CarPooling.Data.Models.Abstract;
using CarPooling.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Models
{
    public class TripRequest : Entity
    {
        public TripRequest()
        { }
        public TripRequest( string driverId, int travelId)
            : base()
        {
            this.DriverId = driverId;
            this.TravelId = travelId;
            this.Status = TripRequestEnum.Pending;
        }

        public TripRequestEnum Status { get; set; } // Pending, Approved, Declined

        // Foreign keys with navigation properties
        public User Driver { get; set; }
        public string DriverId { get; set; }

        public Travel Travel { get; set; }
        public int TravelId { get; set; }
    }
}
