using CarPooling.Data.Models.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Models
{
    public class Car : Entity
    {
        public Car()
        {
        }

        public Car(string userId, string registration, string brand, string model, string color, bool canSmoke, int totalSeats)
            : base()
        {
            this.UserId = userId;
            this.Registration = registration;
            this.TotalSeats = totalSeats;
            this.Brand = brand;
            this.Model = model;
            this.Color = color;
            this.CanSmoke = canSmoke;
        }

        [Required, MinLength(6), MaxLength(10)]
        public string Registration { get; set; }
        public int TotalSeats { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public bool CanSmoke { get; set; }


        // Foreign keys with navigation properties
        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<Travel> Travels { get; set; } = new List<Travel>();
    }
}
