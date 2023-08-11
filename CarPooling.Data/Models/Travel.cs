﻿using CarPooling.Data.Models.Abstract;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace CarPooling.Data.Models
{
    public class Travel : EntityBase
    {
     
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
        public double EstimatedTravelDuration { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public int TravelDistance { get; set; }

        //  public bool? IsCompleted { get; set; }
        public bool? IsCompleted { get; set; }

       

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
