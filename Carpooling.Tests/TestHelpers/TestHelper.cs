using CarPooling.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.Tests.TestHelpers
{
    public static class TestHelper
    {

        public static Country GetTestCountryOne()
        {
            return new Country
            {
                Id = 1,
                Name = "Test Country One",
                IsDeleted = false
            };
        }
        public static Country GetTestCountryTwo()
        {
            return new Country
            {
                Id = 2,
                Name = "Test Country Two",
                IsDeleted = false
            };
        }
        public static Country GetTestCountryThreeSoftDeleted()
        {
            return new Country
            {
                Id = 3,
                Name = "Test Country Two",
                IsDeleted = true
            };
        }
        public static List<Country> GetTestCountries()
        {
            return new List<Country>
            {
                new Country { Id = 1, IsDeleted = false },
                new Country { Id = 2, IsDeleted = true },
                new Country { Id = 3, IsDeleted = false }
            };
        }

        public static User GetTestUserOne()
        {
            User user = new User
            {
                Id = "1",
                FirstName = "Test User One",
                LastName = "Test User One",
                Email = "testOne@gmail.com",
                UserName = "testOne@gmail.com",
                IsBlocked = false,
                Cars = new List<Car> { GetTestCarThree(), GetTestCarOne() }
            };
            return user;
        }
        public static User GetTestUserTwo()
        {
            User user = new User
            {
                Id="2",
                FirstName = "Test User Two",
                LastName = "Test User Two",
                Email = "testTwo@gmail.com",
                UserName = "testTwo@gmail.com",
                IsBlocked = false,
                
            };
            return user;
        }
        public static User GetTestUserThreeSoftDeleted()
        {
            User user = new User
            {
                FirstName = "Test User Three",
                LastName = "Test User Three",
                Email = "testThree@gmail.com",
                UserName = "testThree@gmail.com",
                IsBlocked = false
            };
            //_userManager.AddToRoleAsync(user, "Passenger").GetAwaiter().GetResult();
            return user;
        }
        public static User GetTestUserFourBlocked()
        {
            User user = new User
            {
                FirstName = "Test User Three",
                LastName = "Test User Three",
                Email = "testThree@gmail.com",
                UserName = "testThree@gmail.com",
                IsBlocked = true
            };
            //_userManager.AddToRoleAsync(user, "Passenger").GetAwaiter().GetResult();
            return user;
        }

        public static Car GetTestCarOne()
        {
            return new Car
            {
                Driver = new User
                {
                    Id="1",
                    FirstName = "Test User One",
                    LastName = "Test User One",
                    Email = "testOne@gmail.com",
                    UserName = "testOne@gmail.com",
                    IsBlocked = false
                },
                AvailableSeats = 4,
                Color = "Red",
                Model = "Q5",
                Brand = "Audi",
                TotalSeats = 4,
                CanSmoke = false,
                Registration = "CB3753XM",
                IsDeleted = false,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };

        }
        public static Car GetTestCarTwo()
        {
            return new Car
            {
                Id=2,
                Driver = new User
                {
                    Id="TestUserOneId",
                    FirstName = "Test User One",
                    LastName = "Test User One",
                    Email = "testOne@gmail.com",
                    UserName = "testOne@gmail.com",
                    IsBlocked = false
                },
                AvailableSeats = 4,
                Color = "G-Class",
                Model = "Q5",
                Brand = "Mercedes",
                TotalSeats = 4,
                CanSmoke = false,
                Registration = "CB3334CC",
                IsDeleted = false,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };
        }
        public static Car GetTestCarThree()
        {
            return new Car
            {
                Id=3,
                Driver = new User
                {
                    Id="1",
                    FirstName = "Test User One",
                    LastName = "Test User One",
                    Email = "testOne@gmail.com",
                    UserName = "testOne@gmail.com",
                    IsBlocked = false
                },
                AvailableSeats = 4,
                Color = "G-Class",
                Model = "Q5",
                Brand = "Mercedes",
                TotalSeats = 4,
                CanSmoke = false,
                Registration = "CB6542KH",
                IsDeleted = false,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };
        }
        public static List<Car> GetTestCars()
        {
            return new List<Car>
            {
                 new Car
                 {
                        Id=1,
                        Driver = new User
                        {
                            FirstName = "Test User One",
                            LastName = "Test User One",
                            Email = "testOne@gmail.com",
                            UserName = "testOne@gmail.com",
                            IsBlocked = false
                        },
                        AvailableSeats = 4,
                        Color = "G-Class",
                        Model = "A25",
                        Brand = "Mercedes",
                        TotalSeats = 4,
                        CanSmoke = false,
                        Registration = "CB6542KH",
                        IsDeleted = false,
                        CreatedOn = new DateTime(2023, 8, 15),
                        UpdatedOn = DateTime.Now
                 },
                 new Car
                 {
                     Id=2,
                    Driver = new User
                    {
                        FirstName = "Test User One",
                        LastName = "Test User One",
                        Email = "testOne@gmail.com",
                        UserName = "testOne@gmail.com",
                        IsBlocked = false
                    },
                     AvailableSeats = 4,
                     Color = "Red",
                     Model = "B5",
                     Brand = "Audi",
                     TotalSeats = 4,
                     CanSmoke = false,
                     Registration = "CB3753XM",
                     IsDeleted = false,
                     CreatedOn = new DateTime(2023, 8, 13),
                     UpdatedOn = DateTime.Now
                 },
                 new Car
                 {
                     Id=3,
                     Driver = new User
                     {
                         FirstName = "Test User One",
                         LastName = "Test User One",
                         Email = "testOne@gmail.com",
                         UserName = "testOne@gmail.com",
                         IsBlocked = false
                     },
                     AvailableSeats = 4,
                     Color = "G-Class",
                     Model = "C5",
                     Brand = "Mercedes",
                     TotalSeats = 4,
                     CanSmoke = false,
                     Registration = "CB3334CC",
                     IsDeleted = false,
                     CreatedOn = new DateTime(2023, 8, 14),
                     UpdatedOn = DateTime.Now
                 }
            };
        }
        public static List<Car> GetTestCarsFilteredByCreateTime()
        {
            return new List<Car>
            {
                 new Car
                 {
                        Driver = new User
                        {
                            FirstName = "Test User One",
                            LastName = "Test User One",
                            Email = "testOne@gmail.com",
                            UserName = "testOne@gmail.com",
                            IsBlocked = false
                        },
                        AvailableSeats = 4,
                        Color = "G-Class",
                        Model = "A25",
                        Brand = "Mercedes",
                        TotalSeats = 4,
                        CanSmoke = false,
                        Registration = "CB6542KH",
                        IsDeleted = false,
                        CreatedOn = new DateTime(2023, 8, 15),
                        UpdatedOn = DateTime.Now
                 },
                 new Car
                 {
                     Driver = new User
                     {
                         FirstName = "Test User One",
                         LastName = "Test User One",
                         Email = "testOne@gmail.com",
                         UserName = "testOne@gmail.com",
                         IsBlocked = false
                     },
                     AvailableSeats = 4,
                     Color = "G-Class",
                     Model = "C5",
                     Brand = "Mercedes",
                     TotalSeats = 4,
                     CanSmoke = false,
                     Registration = "CB3334CC",
                     IsDeleted = false,
                     CreatedOn = new DateTime(2023, 8, 14),
                     UpdatedOn = DateTime.Now
                 },
                 new Car
                 {
                    Driver = new User
                    {
                        FirstName = "Test User One",
                        LastName = "Test User One",
                        Email = "testOne@gmail.com",
                        UserName = "testOne@gmail.com",
                        IsBlocked = false
                    },
                     AvailableSeats = 4,
                     Color = "Red",
                     Model = "B5",
                     Brand = "Audi",
                     TotalSeats = 4,
                     CanSmoke = false,
                     Registration = "CB3753XM",
                     IsDeleted = false,
                     CreatedOn = new DateTime(2023, 8, 13),
                     UpdatedOn = DateTime.Now
                 }
            };
        }
        public static List<Car> GetTestCarsFilteredByModel()
        {
            return new List<Car>
            {
                 new Car
                 {
                        Driver = new User
                        {
                            FirstName = "Test User One",
                            LastName = "Test User One",
                            Email = "testOne@gmail.com",
                            UserName = "testOne@gmail.com",
                            IsBlocked = false
                        },
                        AvailableSeats = 4,
                        Color = "G-Class",
                        Model = "A25",
                        Brand = "Mercedes",
                        TotalSeats = 4,
                        CanSmoke = false,
                        Registration = "CB6542KH",
                        IsDeleted = false,
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now
                 },
                  new Car
                 {
                    Driver = new User
                    {
                        FirstName = "Test User One",
                        LastName = "Test User One",
                        Email = "testOne@gmail.com",
                        UserName = "testOne@gmail.com",
                        IsBlocked = false
                    },
                     AvailableSeats = 4,
                     Color = "Red",
                     Model = "B5",
                     Brand = "Audi",
                     TotalSeats = 4,
                     CanSmoke = false,
                     Registration = "CB3753XM",
                     IsDeleted = false,
                     CreatedOn = DateTime.Now,
                     UpdatedOn = DateTime.Now
                 },
                 new Car
                 {
                     Driver = new User
                     {
                         FirstName = "Test User One",
                         LastName = "Test User One",
                         Email = "testOne@gmail.com",
                         UserName = "testOne@gmail.com",
                         IsBlocked = false
                     },
                     AvailableSeats = 4,
                     Color = "G-Class",
                     Model = "C5",
                     Brand = "Mercedes",
                     TotalSeats = 4,
                     CanSmoke = false,
                     Registration = "CB3334CC",
                     IsDeleted = false,
                     CreatedOn = DateTime.Now,
                     UpdatedOn = DateTime.Now
                 }
                
            };
        }
        public static List<Car> GetTestCarsFilteredByBrand()
        {
            return new List<Car>
            {
                 new Car
                 {
                    Driver = new User
                    {
                        FirstName = "Test User One",
                        LastName = "Test User One",
                        Email = "testOne@gmail.com",
                        UserName = "testOne@gmail.com",
                        IsBlocked = false
                    },
                     AvailableSeats = 4,
                     Color = "Red",
                     Model = "B5",
                     Brand = "Audi",
                     TotalSeats = 4,
                     CanSmoke = false,
                     Registration = "CB3753XM",
                     IsDeleted = false,
                     CreatedOn = new DateTime(2023, 8, 14),
                     UpdatedOn = DateTime.Now
                 },
                 new Car
                 {
                        Driver = new User
                        {
                            FirstName = "Test User One",
                            LastName = "Test User One",
                            Email = "testOne@gmail.com",
                            UserName = "testOne@gmail.com",
                            IsBlocked = false
                        },
                        AvailableSeats = 4,
                        Color = "G-Class",
                        Model = "A25",
                        Brand = "Mercedes",
                        TotalSeats = 4,
                        CanSmoke = false,
                        Registration = "CB6542KH",
                        IsDeleted = false,
                        CreatedOn = new DateTime(2023, 8, 15),
                        UpdatedOn = DateTime.Now
                 },  
                 new Car
                 {
                     Driver = new User
                     {
                         FirstName = "Test User One",
                         LastName = "Test User One",
                         Email = "testOne@gmail.com",
                         UserName = "testOne@gmail.com",
                         IsBlocked = false
                     },
                     AvailableSeats = 4,
                     Color = "G-Class",
                     Model = "C5",
                     Brand = "Mercedes",
                     TotalSeats = 4,
                     CanSmoke = false,
                     Registration = "CB3334CC",
                     IsDeleted = false,
                     CreatedOn = new DateTime(2023, 8, 13),
                     UpdatedOn = DateTime.Now
                 }

            };
        }


        public static Address GetTestAddressOne()
        {
            return new Address
            {
                Id=1,
                City = "Sofia",
                Details = "Ovcha Kupel",
                Country = new Country
                {
                    Name = "Bulgaria"
                }
            };
        }
        public static Address GetTestAddressTwo()
        {
            return new Address
            {
                Id=2,
                City = "Sofia",
                Details = "Lyulin 5",
                Country = new Country
                {
                    Name = "Bulgaria"
                }
            };
        }
        public static Address GetTestAddressThree()
        {
            return new Address
            {
                Id=3,
                City = "Sofia",
                Details = "Malinova Dolina",
                Country = new Country
                {
                    Name = "Bulgaria"
                }
            };
        }
        public static Address GetTestAddressFour()
        {
            return new Address
            {
                Id=4,
                City = "Pernik",
                Details = "Centar",
                Country = new Country
                {
                    Name = "Bulgaria"
                }
            };
        }
        public static Address GetTestAddressFive()
        {
            return new Address
            {
                Id=5,
                City = "Pazardzhik",
                Details = "Shirok Centar",
                Country = new Country
                {
                    Name = "Bulgaria"
                }
            };
        }
        public static List<Address> GetTestAddresses()
        {
            return new List<Address>
            {
                new Address
                {
                     City = "Pazardzhik",
                     Details = "Shirok Centar",
                     Country = new Country
                     {
                         Name = "Bulgaria"
                     }
                },
                new Address
                {
                     City = "Shumen",
                     Details = "Maxala",
                     Country = new Country
                     {
                         Name = "Bulgaria"
                     }
                },
                new Address
                {
                     City = "Sofia",
                     Details = "Ovcha Kupel",
                     Country = new Country
                     {
                         Name = "Bulgaria"
                     }
                }
            };
        }

    }
}

