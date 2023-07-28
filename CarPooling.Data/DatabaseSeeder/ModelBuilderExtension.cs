using CarPooling.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CarPooling.Data.DatabaseSeeder
{
    public static class ModelBuilderExtension
    {
        public static void Seeder(this ModelBuilder builder)
        {
            builder.Entity<Address>().HasData(
                new Address()
                {
                   Id = 1,
                   City = "Sofia",
                   Details = "Elin Pelin 14",
                   CountryId = 33,
                   CreatedOn = DateTime.UtcNow
                },
                new Address()
                {
                    Id = 2,
                    City = "Berlin",
                    Details = "Genslerstrasse 23",
                    CountryId = 82,
                    CreatedOn = DateTime.UtcNow
                },
                new Address()
                {
                    Id = 3,
                    City = "Barcelona",
                    Details = "Enric Granados 5",
                    CountryId = 218,
                    CreatedOn = DateTime.UtcNow
                },
                new Address()
                {
                    Id = 4,
                    City = "Rim",
                    Details = "Antonio Beccadelli 40",
                    CountryId = 119,
                    CreatedOn = DateTime.UtcNow
                },
                new Address()
                {
                    Id = 5,
                    City = "Rim",
                    Details = "Via del Corso",
                    CountryId = 119,
                    CreatedOn = DateTime.UtcNow
                }
            );
            builder.Entity<Car>().HasData(
                new Car()
                {
                    Id = 1,
                  // DriverId =?
                    Registration = "BT8878KA",
                    TotalSeats = 4,
                    AvailableSeats = 4,
                    Brand = "Mercedes",
                    Model = "E320",
                    Color = "Black",
                    CanSmoke = false,
                },
                new Car()
                {
                    Id = 2,
                   // DriverId =?
                    Registration = "A5574AK",
                    TotalSeats = 4,
                    AvailableSeats = 2,
                    Brand = "Audi",
                    Model = "80",
                    Color = "Red",
                    CanSmoke = true,
                    CreatedOn = DateTime.UtcNow
                },
                new Car()
                {
                    Id = 3,
                   // DriverId =?
                    Registration = "C5512BT",
                    TotalSeats = 4,
                    AvailableSeats = 1,
                    Brand = "BMW",
                    Model = "730",
                    Color = "Silver",
                    CanSmoke = true,
                    CreatedOn = DateTime.UtcNow
                },
                new Car()
                {
                    Id = 4,
                   // DriverId =?
                    Registration = "CO7777CB",
                    TotalSeats = 4,
                    AvailableSeats = 4,
                    Brand = "Audi",
                    Model = "Q7",
                    Color = "Black",
                    CanSmoke = false,
                    CreatedOn = DateTime.UtcNow
                }           
            );

        }
    }
}
