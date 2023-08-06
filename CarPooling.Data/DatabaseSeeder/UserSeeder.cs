using CarPooling.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CarPooling.Data.DatabaseSeeder
{
    public static class UserSeeder
    {
        public static async Task UserSeeding(this IApplicationBuilder application)
        {
            using (var scope = application.ApplicationServices.CreateScope())
            {
                UserManager<User> _userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var userGosho = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "Gosho89",
                    FirstName = "Gosho",
                    LastName = "Goshev",
                    Email = "gosho@gmail.com",
                    AverageRating = 4.5m,
                    IsBlocked = false
                };
                var userPesho = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "PeshoRace",
                    FirstName = "Pesho",
                    LastName = "Peshev",
                    Email = "pesho@gmail.com",
                    AverageRating = 5m,
                    IsBlocked = false
                };
                var userMisho = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "Misho89",
                    FirstName = "Misho",
                    LastName = "Mishev",
                    Email = "misho@gmail.com",
                    AverageRating = 4.5m,
                    IsBlocked = false
                };
                var userAngel = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "Acho",
                    FirstName = "Angel",
                    LastName = "Angelov",
                    Email = "angel@gmail.com",
                    AverageRating = 4.5m,
                    IsBlocked = false
                };
                var userVanjo = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "Vankata",
                    FirstName = "Vanjo",
                    LastName = "Vanchev",
                    Email = "vanjo@gmail.com",
                    AverageRating = 4.5m,
                    IsBlocked = false
                };
                var userAlex = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "Sasheto",
                    FirstName = "Alex",
                    LastName = "Alexandrev",
                    Email = "alex@gmail.com",
                    AverageRating = 4.5m,
                    IsBlocked = false
                };
                var user7 = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "AlexP",
                    FirstName = "Aleksei",
                    LastName = "Alexandrev",
                    Email = "liulin@gmail.com",
                    AverageRating = 5m,
                    IsBlocked = false
                };
                var user8 = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "Siso",
                    FirstName = "Aleko",
                    LastName = "Alexandrev",
                    Email = "nargile@abv.bg",
                    AverageRating = 5m,
                    IsBlocked = false
                };
                var user9 = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "Vlado",
                    FirstName = "Valcho",
                    LastName = "Alexandrev",
                    Email = "nargile@abv.bg",
                    AverageRating = 5m,
                    IsBlocked = false
                };
                var user10 = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "AudiFen",
                    FirstName = "Dimitar",
                    LastName = "Alexandrev",
                    Email = "audi@abv.bg",
                    AverageRating = 5m,
                    IsBlocked = false
                };
                var user11 = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "SamoCSKA",
                    FirstName = "Alexandropolis",
                    LastName = "Alexandrev",
                    Email = "VW@abv.bg",
                    AverageRating = 5m,
                    IsBlocked = false
                };
                var user12 = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "Robin",
                    FirstName = "Robert",
                    LastName = "Alexandrev",
                    Email = "Passat@abv.bg",
                    AverageRating = 4m,
                    IsBlocked = false
                };
                var user13 = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "Iliya123",
                    FirstName = "Iliya",
                    LastName = "Spiridonov",
                    Email = "Golf@abv.bg",
                    AverageRating = 3m,
                    IsBlocked = false
                };
                var user14 = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "CoolTrip",
                    FirstName = "Valio",
                    LastName = "Spiridonov",
                    Email = "Mazda@abv.bg",
                    AverageRating = 5m,
                    IsBlocked = false
                };
                var user15 = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "CoolCar",
                    FirstName = "Trendafil",
                    LastName = "Trendafilov",
                    Email = "Gaz@abv.bg",
                    AverageRating = 5m,
                    IsBlocked = false
                };
                var user16 = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "BestDriver",
                    FirstName = "Kosio",
                    LastName = "Trendafilov",
                    Email = "Gaz123@abv.bg",
                    AverageRating = 5m,
                    IsBlocked = false
                };
                var user17 = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "HotChick",
                    FirstName = "Ralica",
                    LastName = "Georgieva",
                    Email = "HotChick@abv.bg",
                    AverageRating = 5m,
                    IsBlocked = false
                };
                var user18 = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "Ani56",
                    FirstName = "Ani",
                    LastName = "Andreeva",
                    Email = "Ani56@abv.bg",
                    AverageRating = 3.5m,
                    IsBlocked = false
                };
                var user19 = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "FastAndFurious",
                    FirstName = "Niki",
                    LastName = "Nikolaev",
                    Email = "Bmw@abv.bg",
                    AverageRating = 4.5m,
                    IsBlocked = false
                };
                var user20 = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "SuperWoman",
                    FirstName = "Milena",
                    LastName = "Milenova",
                    Email = "Nissan@gmail.bg",
                    AverageRating = 4.5m,
                    IsBlocked = false
                };
                var user21 = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "AngryMan",
                    FirstName = "Damqn",
                    LastName = "Damqnov",
                    Email = "DDDD@gmail.bg",
                    AverageRating = 4m,
                    IsBlocked = false
                };
                var password = "SamplePassword123!";
                var result1 = await _userManager.CreateAsync(userGosho, password);
                var result2 = await _userManager.CreateAsync(userPesho, password);
                var result3 = await _userManager.CreateAsync(userMisho, password);
                var result4 = await _userManager.CreateAsync(userAlex, password);
                var result5 = await _userManager.CreateAsync(userVanjo, password);
                var result6 = await _userManager.CreateAsync(userAngel, password);
                var result7 = await _userManager.CreateAsync(user7, password);
                var result8 = await _userManager.CreateAsync(user8, password);
                var result9 = await _userManager.CreateAsync(user9, password);
                var result10 = await _userManager.CreateAsync(user10, password);
                var result11 = await _userManager.CreateAsync(user11, password);
                var result12 = await _userManager.CreateAsync(user12, password);
                var result13 = await _userManager.CreateAsync(user13, password);
                var result14 = await _userManager.CreateAsync(user14, password);
                var result15 = await _userManager.CreateAsync(user15, password);
                var result16 = await _userManager.CreateAsync(user16, password);
                var result17 = await _userManager.CreateAsync(user17, password);
                var result18 = await _userManager.CreateAsync(user18, password);
                var result19 = await _userManager.CreateAsync(user19, password);
                var result20 = await _userManager.CreateAsync(user20, password);
                var result21 = await _userManager.CreateAsync(user21, password);

                if (result1.Succeeded)
                {
                    await _userManager.AddToRoleAsync(userGosho, "Administrator");
                }
                if (result2.Succeeded)
                {
                    await _userManager.AddToRoleAsync(userPesho, "Driver");
                }
                if (result3.Succeeded)
                {
                    await _userManager.AddToRoleAsync(userMisho, "Passenger");
                }
                if (result4.Succeeded)
                {
                    await _userManager.AddToRoleAsync(userAlex, "Driver");
                }
                if (result5.Succeeded)
                {
                    await _userManager.AddToRoleAsync(userVanjo, "Passenger");
                }
                if (result6.Succeeded)
                {
                    await _userManager.AddToRoleAsync(userAngel, "Driver");
                }
                if (result7.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user7, "Passenger");
                }
                if (result8.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user8, "Driver");
                }
                if (result9.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user9, "Passenger");
                }
                if (result10.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user10, "Driver");
                } 
                if (result11.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user11, "Passenger");
                }
                if (result12.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user12, "Driver");
                }
                if (result13.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user13, "Passenger");
                }
                if (result14.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user14, "Driver");
                }
                if (result15.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user15, "Passenger");
                }
                if (result16.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user16, "Driver");
                }
                if (result17.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user17, "Passenger");
                }
                if (result18.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user18, "Driver");
                }
                if (result19.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user19, "Passenger");
                }
                if (result20.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user20, "Driver");
                }
                if (result21.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user21, "Passenger");
                }
            }
            
        }
    }
}
