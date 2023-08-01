using CarPooling.Data.Models.Abstract;
using CarPooling.Data.Models.Enums;

namespace CarPooling.Data.Models
{
    public class TripRequest : EntityBase
    {
        public TripRequest()
        { }

        public TripRequest(string authorId, int travelId)
          : base()
        {
            this.PassengerId = authorId;
            this.TravelId = travelId;
            this.Status = TripRequestEnum.Pending;

        }

        public TripRequestEnum Status { get; set; } // Pending, Approved, Declined


        // Foreign keys with navigation properties
        public User Passenger { get; set; }
        public string PassengerId { get; set; }

        public Travel Travel { get; set; }
        public int TravelId { get; set; }
    }
}
