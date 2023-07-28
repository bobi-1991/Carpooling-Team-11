using CarPooling.Data.Data;
using CarPooling.Data.Models;
using CarPooling.Data.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.DatabaseSeeder
{
    public static class ModelBuilderExtension
    {

        public static void Seeder(this ModelBuilder builder)
        {
            //builder.Entity<Address>().HasData(
            //    new Address
            //    {
            //        Id = 1,
            //        City = "Plovdiv",
            //        CountryId = 1,
            //        Details = "Kapana",
            //        CreatedOn = DateTime.Now
            //    },
            //    new Address
            //    {
            //        Id = 2,
            //        City = "Varna",
            //        CountryId = 1,
            //        Details = "Kapana",
            //        CreatedOn = DateTime.Now
            //    },
            //    new Address
            //    {
            //        Id = 3,
            //        City = "Sofia",
            //        CountryId = 1,
            //        Details = "Ovcha kupel",
            //        CreatedOn = DateTime.Now
            //    }
            //    );

            //builder.Entity<Feedback>().HasData(
            //    new Feedback
            //    {
            //        Id = 1,
            //        DriverId = "4f7bbc96-7347-48db-92d5-25360308fa40",
            //        PassengerId = "a163bc77-0f37-4d00-8e04-9bd42b89f41d",
            //        TravelId = 2,
            //        Rating = 10,
            //        Comment = "Very good travel, excel;ent Driver!"
            //    },
            //    new Feedback
            //    {
            //        Id = 2,
            //        DriverId = "9561adbf-2228-4435-b049-65b2033b2a21",
            //        PassengerId = "e6224dee-e6ed-468d-a3ed-1750c28bc133",
            //        TravelId = 3,
            //        Rating = 8,
            //    },
            //    new Feedback
            //    {
            //        Id = 3,
            //        DriverId = "5e1ae774-914d-4bd4-97b4-ef380ad5ed01",
            //        PassengerId= "a163bc77-0f37-4d00-8e04-9bd42b89f41d",
            //        TravelId = 4,
            //        Rating = 1,
            //        Comment = "Refused to put me on board!"
            //    },
            //    new Feedback
            //    {
            //        Id = 4,
            //        DriverId = "9561adbf-2228-4435-b049-65b2033b2a21",
            //        PassengerId = "a163bc77-0f37-4d00-8e04-9bd42b89f41d",
            //        TravelId = 5,
            //        Rating = 6,
            //        Comment = "Not careful when driving."
            //    }
            //    ) ;


            //builder.Entity<Car>().HasData(
            //        new Car
            //        {
            //            Id = 1,
            //            Registration = "CA4921PA",
            //            AvailableSeats = 3,
            //            TotalSeats = 4,
            //            Brand = "BMW",
            //            Model = "X5",
            //            Color = "Skyblue",
            //            DriverId = "4f7bbc96-7347-48db-92d5-25360308fa40",
            //            CanSmoke = true
            //        },
            //        new Car
            //        {
            //            Id = 2,
            //            Registration = "C05943CB",
            //            AvailableSeats = 3,
            //            TotalSeats = 4,
            //            Brand = "Audi",
            //            Model = "A4",
            //            Color = "Black",
            //            DriverId = "5e1ae774-914d-4bd4-97b4-ef380ad5ed01",
            //            CanSmoke = true
            //        },
            //        new Car
            //        {
            //            Id = 3,
            //            Registration = "CAXXXXXX",
            //            AvailableSeats = 4,
            //            TotalSeats = 5,
            //            Brand = "Mercedes",
            //            Model = "Maybach",
            //            Color = "Black",
            //            DriverId = "4f7bbc96-7347-48db-92d5-25360308fa40",
            //            CanSmoke = true
            //        },
            //        new Car
            //        {
            //            Id = 4,
            //            Registration = "PB3562PA",
            //            AvailableSeats = 3,
            //            TotalSeats = 4,
            //            Brand = "BMW",
            //            Model = "X5",
            //            DriverId = "9561adbf-2228-4435-b049-65b2033b2a21",
            //            Color = "Skyblue",
            //            CanSmoke = true
            //        }
            //        );

            //builder.Entity<Travel>().HasData(
            //    new Travel
            //    {
            //        Id = 1,
            //        DriverId = "9561adbf-2228-4435-b049-65b2033b2a21",
            //        //StartLocation = new Address { City = "Sofia", Details = "Alexander Nevski 23" },
            //        //EndLocation = new Address { City = "Burgas", Details = "Morskata Gradina" },
            //        DepartureTime = DateTime.UtcNow,
            //        CreatedOn = DateTime.Now
            //    },

            //    new Travel
            //    {
            //        Id = 2,
            //        DriverId = "4f7bbc96-7347-48db-92d5-25360308fa40",
            //        //StartLocation = new Address {Id=1, CountryId= 1, City = "Pleven", Details = "Kirilovci" },
            //        //EndLocation = new Address { Id = 2, CountryId = 1, City = "Plovdiv", Details = "Kapana" },
            //        DepartureTime = DateTime.UtcNow,
            //        CreatedOn = DateTime.Now
            //    },
            //    new Travel
            //    {
            //        Id = 3,
            //        DriverId = "9561adbf-2228-4435-b049-65b2033b2a21",
            //        //StartLocation = new Address { Id = 3, CountryId = 1, City = "Pazardzhik", Details = "Centur" },
            //        //EndLocation = new Address { Id = 4, CountryId = 1, City = "Slunchev brqg", Details = "Plaja" },
            //        DepartureTime = DateTime.UtcNow,
            //        CreatedOn = DateTime.Now
            //    },
            //    new Travel
            //    {
            //        Id = 4,
            //        DriverId = "5e1ae774-914d-4bd4-97b4-ef380ad5ed01",
            //        //StartLocation = new Address { Id = 5, CountryId = 1, City = "Sandanski", Details = "Centralna Gara" },
            //        //EndLocation = new Address { Id = 6, CountryId = 1, City = "Sofia", Details = "Lyulin 5" },
            //        //CarId = 2,
            //        DepartureTime = DateTime.UtcNow,
            //        CreatedOn = DateTime.Now
            //    },
            //    new Travel
            //    {
            //        Id = 5,
            //        DriverId = "9561adbf-2228-4435-b049-65b2033b2a21",
            //        //StartLocation = new Address { Id = 7, CountryId = 1, City = "Burgas", Details = "Centralna Gara" },
            //        //EndLocation = new Address { Id = 8, CountryId = 1, City = "Varna", Details = "Centyr" },
            //        DepartureTime = DateTime.UtcNow,
            //        CreatedOn = DateTime.Now
            //    }
            //    );

            //builder.Entity<TripRequest>().HasData(
            //    new TripRequest
            //    {
            //        Id = 1,
            //        Status = TripRequestEnum.Approved,
            //        PassengerId = "4f7bbc96-7347-48db-92d5-25360308fa40",
            //        TravelId = 1,
            //        CreatedOn = DateTime.Now
            //    },
            //    new TripRequest
            //    {
            //        Id = 2,
            //        Status = TripRequestEnum.Approved,
            //        PassengerId = "e6224dee-e6ed-468d-a3ed-1750c28bc133",
            //        TravelId = 2,
            //        CreatedOn = DateTime.Now
            //    },
            //    new TripRequest
            //    {
            //        Id = 3,
            //        Status = TripRequestEnum.Pending,
            //        PassengerId = "e6224dee-e6ed-468d-a3ed-1750c28bc133",
            //        TravelId = 3,
            //        CreatedOn = DateTime.Now
            //    },
            //    new TripRequest
            //    {
            //        Id = 4,
            //        Status = TripRequestEnum.Declined,
            //        PassengerId = "a163bc77-0f37-4d00-8e04-9bd42b89f41d",
            //        TravelId = 4,
            //        CreatedOn = DateTime.Now
            //    },
            //    new TripRequest
            //    {
            //        Id = 5,
            //        Status = TripRequestEnum.Approved,
            //        PassengerId = "a163bc77-0f37-4d00-8e04-9bd42b89f41d",
            //        TravelId = 5,
            //        CreatedOn = DateTime.Now
            //    }
            //    );


        }
    }
}
