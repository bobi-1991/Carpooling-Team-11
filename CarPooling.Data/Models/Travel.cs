using CarPooling.Data.Models.Abstract;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace CarPooling.Data.Models
{
    public class Travel : EntityBase
    {
        //public Travel()
        //{
        //}

        //// Foreign keys with navigation properties
        //public User? Driver { get; set; }
        //public string? DriverId { get; set; }

        ////public int? StartLocationId { get; set; }
        //public Address? StartLocation { get; set; }

        ////public int? EndLocationId { get; set; }
        //public Address? EndLocation { get; set; }

        //public DateTime DepartureTime { get; set; }
        //public DateTime ArrivalTime { get; set; }


        //public bool? IsCompleted { get; set; }

        ////public int? CarId { get; set; }
        //public Car? Car { get; set; }

        //public int AvaibleSlots { get; set; }


        //public List<User>? Passengers { get; set; } = new List<User>();
        //public List<Feedback>? Feedbacks { get; set; } = new List<Feedback>();

        private readonly int availableSeats = 4;
        public Travel()
        {
            AvailableSeats = this.availableSeats;
        }


        // Foreign keys with navigation properties
        public User? Driver { get; set; }
        public string? DriverId { get; set; }

        //public int? StartLocationId { get; set; }
        public Address? StartLocation { get; set; }

        //public int? EndLocationId { get; set; }
        public Address? EndLocation { get; set; }

        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }


        //  public bool? IsCompleted { get; set; }
        public bool? IsCompleted
        {
            get => DepartureTime <= DateTime.Now;
            set { }
        }
       

        //public int? CarId { get; set; }
        public Car? Car { get; set; }

        public int? AvailableSeats
        {
            get; set;
        }

        public List<User>? Passengers = new List<User>();


        public List<Feedback>? Feedbacks { get; set; } = new List<Feedback>();




    }

}
