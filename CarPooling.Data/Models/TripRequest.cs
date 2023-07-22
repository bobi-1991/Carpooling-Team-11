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
        public TripRequest(int authorId, int driverId, int travelId)
            : base()
        {
            this.AuthorId = authorId;
            this.DriverId = driverId;
            this.TravelId = travelId;
            this.Status = TripRequestEnum.Pending;
        }

        public TripRequestEnum Status { get; set; } // Pending, Approved, Declined


        // Foreign keys with navigation properties
        public User Author { get; set; }
        public int AuthorId { get; set; }

        public User Driver { get; set; }
        public int DriverId { get; set; }

        public Travel Travel { get; set; }
        public int TravelId { get; set; }
    }
}
