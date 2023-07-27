using CarPooling.Data.Models.Abstract;
using Microsoft.AspNetCore.Identity;

namespace CarPooling.Data.Models
{
    public class User:IdentityUser
    {
        public User()
        {
            
        }
      //  public string Id { get; set; }
         public bool IsDeleted { get; set; }
        //public DateTime CreateOn { get; set; }
        //public DateTime UpdatedOn { get; set; }
        //public DateTime DeleteOn { get; set; }


       // public string Username { get; set; }
       // public string Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public decimal AverageRating { get; set; }
        public string? ProfileImage { get; set; }
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

        //public User Update(User user)
        //{
        //    this.FirstName = user.FirstName ?? FirstName;
        //    this.LastName = user.LastName ?? LastName;
        //    this.Password = user.Password ?? Password;
        //    this.Email = user.Email ?? Email;
        //    this.UpdatedOn = DateTime.UtcNow;

        //    return this;
        //}
    }
}
