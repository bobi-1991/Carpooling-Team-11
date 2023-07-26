using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CarPooling.Data.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            
        }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public decimal AverageRating { get; set; }
        public string? ProfileImage { get; set; }
        public bool IsBlocked { get; set; }

        // IdentityUser ??
        public bool IsAdmin { get; set; }


        // Foreign keys with navigation properties
        //public int TravelId { get; set; }
        public Travel? Travel { get; set; }
        
        public int AddressId { get; set; }
        public Address? Address { get; set;}
        
        public int CityId { get; set; }
        public City? City { get; set; }

        public ICollection<Feedback> AuthorFeedbacks { get; set; } = new List<Feedback>();
        public ICollection<Feedback> RecipientFeedbacks { get; set; } = new List<Feedback>();
        public ICollection<TripRequest> AuthorTripRequests { get; set; } = new List<TripRequest>();
        public ICollection<TripRequest> RecipientTripRequests { get; set; } = new List<TripRequest>();
        public ICollection<Travel> TravelHistory { get; set; } = new List<Travel>();
        public ICollection<Car> Cars { get; set; } = new List<Car>();
         
    }
}
