using CarPooling.Data.Data;
using CarPooling.Data.JsonManager;
using CarPooling.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CarPooling.Data.DatabaseSeeder
{
    public static class TravelSeeder
    {
        public static void SeedDatabaseTravel(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var _context = serviceScope.ServiceProvider.GetService<CarPoolingDbContext>();
                var _jsonManager = serviceScope.ServiceProvider.GetService<IJsonManager>();

                var driver = _context.Users.FirstOrDefault(x => x.FirstName.Equals("Gosho"));
                var driver2 = _context.Users.FirstOrDefault(x => x.FirstName.Equals("Alex"));
                var driver3 = _context.Users.FirstOrDefault(x => x.FirstName.Equals("Pesho"));
                var passenger1 = _context.Users.FirstOrDefault(x => x.FirstName.Equals("Misho"));
                var passenger2 = _context.Users.FirstOrDefault(x => x.FirstName.Equals("Angel"));

                var car = new Car
                {
                    Driver = driver,
                    AvailableSeats = 4,
                    Color = "Red",
                    Model = "Q5",
                    Brand = "Audi",
                    TotalSeats = 4,
                    CanSmoke = false,
                    Registration = "CB2928XM",
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now
                };

                var car2 = new Car
                {
                    Driver = driver2,
                    AvailableSeats = 4,
                    Color = "Black",
                    Model = "X5",
                    Brand = "BMW",
                    TotalSeats = 4,
                    CanSmoke = false,
                    Registration = "BT7777TP",
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now
                };

                var car3 = new Car
                {
                    Driver = driver3,
                    AvailableSeats = 4,
                    Color = "Silver",
                    Model = "735",
                    Brand = "BMW",
                    TotalSeats = 4,
                    CanSmoke = false,
                    Registration = "BT8888KT",
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now
                };

                //             public string Registration { get; set; }
                //public int TotalSeats { get; set; }
                //public int AvailableSeats { get; set; }
                //public string Brand { get; set; }
                //public string Model { get; set; }
                //public string Color { get; set; }
                //public bool CanSmoke { get; set; }

                //// Foreign keys with navigation properties
                //public string DriverId { get; set; }
                //public User Driver { get; set; }
                if (_context.Cars.Count() == 0)
                {
                    if (!_context.Cars.Any(x => x.Registration == car.Registration))
                    {
                        _context.Cars.Add(car);
                        _context.SaveChanges();
                    }
                    if (!_context.Cars.Any(x => x.Registration == car2.Registration))
                    {
                        _context.Cars.Add(car2);
                        _context.SaveChanges();
                    }
                    if (!_context.Cars.Any(x => x.Registration == car3.Registration))
                    {
                        _context.Cars.Add(car3);
                        _context.SaveChanges();
                    }
                }
                var startLocaion = _context.Addresses.FirstOrDefault(a => a.Id == 1);
                var endLocation = _context.Addresses.FirstOrDefault(a => a.Id == 1);

                var travel = new Travel
                {
                    Car = car,
                    DriverId = driver.Id,
                    Driver = driver,
                    StartLocation = startLocaion,
                    EndLocation = endLocation,
                    DepartureTime = new DateTime(2023, 10, 12),
                    ArrivalTime = new DateTime(2023, 10, 13),
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    AvailableSeats = 4,
                    Passengers = new List<User> { passenger1, passenger2 },
                    IsCompleted = false,
                    Feedbacks = new List<Feedback> { new Feedback {PassengerId = passenger1.Id, Passenger = passenger1,  DriverId = driver.Id, Rating = 5, Comment = "It was fine." } }
                };

                _context.Travels.Add(travel);
                _context.SaveChanges();

            }
        }
    }
}
