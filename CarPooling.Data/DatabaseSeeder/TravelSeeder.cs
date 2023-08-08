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

                var driver4 = _context.Users.FirstOrDefault(x => x.FirstName.Equals("Kosio"));
                var driver5 = _context.Users.FirstOrDefault(x => x.FirstName.Equals("Aleko"));
                var driver6 = _context.Users.FirstOrDefault(x => x.FirstName.Equals("Dimitar"));


                var passenger1 = _context.Users.FirstOrDefault(x => x.FirstName.Equals("Misho"));
                var passenger2 = _context.Users.FirstOrDefault(x => x.FirstName.Equals("Angel"));

                var passenger3 = _context.Users.FirstOrDefault(x => x.FirstName.Equals("Vanjo"));
                var passenger4 = _context.Users.FirstOrDefault(x => x.FirstName.Equals("Aleksei"));
                var passenger5 = _context.Users.FirstOrDefault(x => x.FirstName.Equals("Valcho"));
                var passenger6 = _context.Users.FirstOrDefault(x => x.FirstName.Equals("Alexandropolis"));
                var passenger7 = _context.Users.FirstOrDefault(x => x.FirstName.Equals("Iliya"));
                var passenger8 = _context.Users.FirstOrDefault(x => x.FirstName.Equals("Trendafil"));
                var passenger9 = _context.Users.FirstOrDefault(x => x.FirstName.Equals("Ralica"));
                var passenger10 = _context.Users.FirstOrDefault(x => x.FirstName.Equals("Niki"));
                var passenger11 = _context.Users.FirstOrDefault(x => x.FirstName.Equals("Damqn"));


                

                var car = new Car
                {
                    Driver = driver,
                    DriverId = driver.Id,
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
                driver.Cars.Add(car);
                
                var car2 = new Car
                {
                    Driver = driver2,
                    DriverId = driver2.Id,
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
                driver2.Cars.Add(car2);
                var car3 = new Car
                {
                    Driver = driver3,
                    DriverId = driver3.Id,
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
                driver3.Cars.Add(car3);
                var car4 = new Car
                {
                    Driver = driver4,
                    DriverId = driver4.Id,
                    AvailableSeats = 4,
                    Color = "Silver",
                    Model = "Q7",
                    Brand = "Audi",
                    TotalSeats = 4,
                    CanSmoke = false,
                    Registration = "CB5491OM",
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now
                };
                driver4.Cars.Add(car4);
                var car5 = new Car
                {
                    Driver = driver5,
                    DriverId = driver5.Id,
                    AvailableSeats = 4,
                    Color = "Black",
                    Model = "X3",
                    Brand = "BMW",
                    TotalSeats = 4,
                    CanSmoke = false,
                    Registration = "A0541XA",
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now
                };
                driver5.Cars.Add(car5);
                var car6 = new Car
                {
                    Driver = driver6,
                    DriverId = driver6.Id,
                    AvailableSeats = 4,
                    Color = "Red",
                    Model = "320",
                    Brand = "BMW",
                    TotalSeats = 4,
                    CanSmoke = false,
                    Registration = "PB5046KP",
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now
                };
                driver6.Cars.Add(car6);
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


                    if (!_context.Cars.Any(x => x.Registration == car3.Registration))
                    {
                        _context.Cars.Add(car4);
                        _context.SaveChanges();
                    }
                    if (!_context.Cars.Any(x => x.Registration == car3.Registration))
                    {
                        _context.Cars.Add(car5);
                        _context.SaveChanges();
                    }
                    if (!_context.Cars.Any(x => x.Registration == car3.Registration))
                    {
                        _context.Cars.Add(car6);
                        _context.SaveChanges();
                    }
                }

                var startLocaion = _context.Addresses.FirstOrDefault(a => a.Id == 1);
                var endLocation = _context.Addresses.FirstOrDefault(a => a.Id == 1);

                var startLocaion2 = _context.Addresses.FirstOrDefault(a => a.Id == 2);
                var endLocation2 = _context.Addresses.FirstOrDefault(a => a.Id == 3);

                var startLocaion3 = _context.Addresses.FirstOrDefault(a => a.Id == 3);
                var endLocation3 = _context.Addresses.FirstOrDefault(a => a.Id == 2);

                var startLocaion4 = _context.Addresses.FirstOrDefault(a => a.Id == 4);
                var endLocation4 = _context.Addresses.FirstOrDefault(a => a.Id == 5);

                var startLocaion5 = _context.Addresses.FirstOrDefault(a => a.Id == 10);
                var endLocation5 = _context.Addresses.FirstOrDefault(a => a.Id == 12);

                var startLocaion6 = _context.Addresses.FirstOrDefault(a => a.Id == 11);
                var endLocation6 = _context.Addresses.FirstOrDefault(a => a.Id == 11);

                var startLocaion7 = _context.Addresses.FirstOrDefault(a => a.Id == 13);
                var endLocation7 = _context.Addresses.FirstOrDefault(a => a.Id == 14);

                var startLocaion8 = _context.Addresses.FirstOrDefault(a => a.Id == 8);
                var endLocation8 = _context.Addresses.FirstOrDefault(a => a.Id == 3);

                var startLocaion9 = _context.Addresses.FirstOrDefault(a => a.Id == 1);
                var endLocation9 = _context.Addresses.FirstOrDefault(a => a.Id == 2);

                var startLocaion10 = _context.Addresses.FirstOrDefault(a => a.Id == 3);
                var endLocation10 = _context.Addresses.FirstOrDefault(a => a.Id == 4);

                var startLocaion11 = _context.Addresses.FirstOrDefault(a => a.Id == 7);
                var endLocation11 = _context.Addresses.FirstOrDefault(a => a.Id == 11);

                var startLocaion12 = _context.Addresses.FirstOrDefault(a => a.Id == 1);
                var endLocation13 = _context.Addresses.FirstOrDefault(a => a.Id == 3);

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
                    AvailableSeats = 2,
                    Passengers = new List<User> { passenger1, passenger2 },
                    IsCompleted = false,
                    Feedbacks = new List<Feedback> { new Feedback { PassengerId = passenger1.Id, Passenger = passenger2, DriverId = driver.Id, Rating = 5, Comment = "It was fine." } }
                };

                var travel2 = new Travel
                {
                    Car = car2,
                    DriverId = driver2.Id,
                    Driver = driver2,
                    StartLocation = startLocaion2,
                    EndLocation = endLocation2,
                    DepartureTime = new DateTime(2023, 10, 12),
                    ArrivalTime = new DateTime(2023, 10, 13),
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    AvailableSeats = 2,
                    Passengers = new List<User> { passenger3, passenger4 },
                    IsCompleted = false,
                    Feedbacks = new List<Feedback> { new Feedback { PassengerId = passenger3.Id, Passenger = passenger4, DriverId = driver2.Id, Rating = 5, Comment = "It was fine." } }
                };

                var travel3 = new Travel
                {
                    Car = car3,
                    DriverId = driver3.Id,
                    Driver = driver3,
                    StartLocation = startLocaion3,
                    EndLocation = endLocation3,
                    DepartureTime = new DateTime(2023, 10, 12),
                    ArrivalTime = new DateTime(2023, 10, 13),
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    AvailableSeats = 2,
                    Passengers = new List<User> { passenger5, passenger6 },
                    IsCompleted = false,
                    Feedbacks = new List<Feedback> { new Feedback { PassengerId = passenger5.Id, Passenger = passenger6, DriverId = driver3.Id, Rating = 5, Comment = "It was fine." } }
                };

                var travel4 = new Travel
                {
                    Car = car4,
                    DriverId = driver4.Id,
                    Driver = driver4,
                    StartLocation = startLocaion4,
                    EndLocation = endLocation4,
                    DepartureTime = new DateTime(2023, 10, 12),
                    ArrivalTime = new DateTime(2023, 10, 13),
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    AvailableSeats = 2,
                    Passengers = new List<User> { passenger7, passenger8 },
                    IsCompleted = false,
                    Feedbacks = new List<Feedback> { new Feedback { PassengerId = passenger7.Id, Passenger = passenger8, DriverId = driver4.Id, Rating = 5, Comment = "It was fine." } }
                };

                var travel5 = new Travel
                {
                    Car = car5,
                    DriverId = driver5.Id,
                    Driver = driver5,
                    StartLocation = startLocaion5,
                    EndLocation = endLocation5,
                    DepartureTime = new DateTime(2023, 10, 12),
                    ArrivalTime = new DateTime(2023, 10, 13),
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    AvailableSeats = 2,
                    Passengers = new List<User> { passenger9, passenger10 },
                    IsCompleted = false,
                    Feedbacks = new List<Feedback> { new Feedback { PassengerId = passenger9.Id, Passenger = passenger10, DriverId = driver5.Id, Rating = 5, Comment = "It was fine." } }
                };

                var travel6 = new Travel
                {
                    Car = car6,
                    DriverId = driver6.Id,
                    Driver = driver6,
                    StartLocation = startLocaion6,
                    EndLocation = endLocation6,
                    DepartureTime = new DateTime(2023, 10, 12),
                    ArrivalTime = new DateTime(2023, 10, 13),
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    AvailableSeats = 3,
                    Passengers = new List<User> { passenger11},
                    IsCompleted = false,
                    Feedbacks = new List<Feedback> { new Feedback { PassengerId = passenger11.Id,  DriverId = driver6.Id, Rating = 5, Comment = "It was fine." } }
                };

                var travel7 = new Travel
                {
                    Car = car6,
                    DriverId = driver6.Id,
                    Driver = driver6,
                    StartLocation = startLocaion7,
                    EndLocation = endLocation7,
                    DepartureTime = new DateTime(2023, 10, 12),
                    ArrivalTime = new DateTime(2023, 10, 13),
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    AvailableSeats = 2,
                    Passengers = new List<User> { passenger1, passenger2 },
                    IsCompleted = false,
                    Feedbacks = new List<Feedback> { new Feedback { PassengerId = passenger1.Id, Passenger = passenger2, DriverId = driver6.Id, Rating = 5, Comment = "It was fine." } }
                };

                var travel8 = new Travel
                {
                    Car = car3,
                    DriverId = driver3.Id,
                    Driver = driver3,
                    StartLocation = startLocaion8,
                    EndLocation = endLocation8,
                    DepartureTime = new DateTime(2023, 11, 01),
                    ArrivalTime = new DateTime(2023, 11, 02),
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    AvailableSeats = 4,
                    Passengers = new List<User>(),
                    IsCompleted = false,
                    Feedbacks = new List<Feedback> { new Feedback { PassengerId = passenger1.Id, Passenger = passenger1, DriverId = driver.Id, Rating = 5, Comment = "It was fine." } }
                };

                var travel9 = new Travel
                {
                    Car = car2,
                    DriverId = driver2.Id,
                    Driver = driver2,
                    StartLocation = startLocaion9,
                    EndLocation = endLocation9,
                    DepartureTime = new DateTime(2023, 08, 03),
                    ArrivalTime = new DateTime(2023, 08, 04),
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    AvailableSeats = 2,
                    Passengers = new List<User> { passenger1, passenger2 },
                    IsCompleted = false,
                    Feedbacks = new List<Feedback> { new Feedback { PassengerId = passenger1.Id, Passenger = passenger1, DriverId = driver.Id, Rating = 5, Comment = "It was fine." } }
                };

                var travel10 = new Travel
                {
                    Car = car,
                    DriverId = driver.Id,
                    Driver = driver,
                    StartLocation = startLocaion10,
                    EndLocation = endLocation10,
                    DepartureTime = new DateTime(2023, 12, 22),
                    ArrivalTime = new DateTime(2023, 12, 23),
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    AvailableSeats = 2,
                    Passengers = new List<User> { passenger1, passenger2 },
                    IsCompleted = false,
                    Feedbacks = new List<Feedback> { new Feedback { PassengerId = passenger1.Id, Passenger = passenger2, DriverId = driver.Id, Rating = 5, Comment = "It was fine." } }
                };

                if (_context.Travels.Count() == 0)
                {
                    _context.Travels.Add(travel);
                    _context.Travels.Add(travel2);
                    _context.Travels.Add(travel3);
                    _context.Travels.Add(travel4);
                    _context.Travels.Add(travel5);
                    _context.Travels.Add(travel6);
                    _context.Travels.Add(travel7);
                    _context.Travels.Add(travel8);
                    _context.Travels.Add(travel9);
                    _context.Travels.Add(travel10);
                    _context.SaveChanges();
                }



            }
        }
    }
}
