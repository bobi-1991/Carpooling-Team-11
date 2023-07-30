﻿using CarPooling.Data.Models.Abstract;
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
        public Travel()
        {
        }

        // Foreign keys with navigation properties
        public User? Driver { get; set; }
        public string? DriverId { get; set; }

        //public int? StartLocationId { get; set; }
        public Address? StartLocation { get; set; }

        //public int? EndLocationId { get; set; }
        public Address? EndLocation { get; set; }

        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }


        public bool? IsCompleted { get; set; }

        //public int? CarId { get; set; }
        public Car? Car { get; set; }

        //public int AvailableSlots
        //{
        //    get
        //    {
        //        // Не позволяваме да имаме отрицателен брой свободни места
        //        return Math.Max(0, AvailableSlots - Passengers.Count());
        //    }
        //    private set
        //    {
        //        AvailableSlots = value;
        //    }
        //}

        public List<User>? Passengers = new List<User>();
    

        public List<Feedback>? Feedbacks { get; set; } = new List<Feedback>();




    }
}
