using CarPooling.Data.Models.Abstract;
using Microsoft.AspNetCore.Identity;

namespace CarPooling.Data.Models
{
    public class User:IdentityUser
    {
        public User()
        {
            
        }
        public bool IsDeleted { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public decimal AverageRating { get; set; }
        public string? ImageURL { get; set; }
        public bool IsBlocked { get; set; }

        // IdentityUser ??
        public bool IsAdmin { get; set; }


        // Foreign keys with navigation properties
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        
        public ICollection<Feedback>? PassengerFeedbacks { get; set; } = new List<Feedback>();
        public ICollection<Feedback>? DriverFeedbacks { get; set; } = new List<Feedback>();
        public ICollection<TripRequest>? PassengerTripRequests { get; set; } = new List<TripRequest>();
        public ICollection<TripRequest>? DriverTripRequests { get; set; } = new List<TripRequest>();
        public ICollection<Travel>? TravelHistory { get; set; } = new List<Travel>();
        public ICollection<Car>? Cars { get; set; } = new List<Car>();

    }
}
