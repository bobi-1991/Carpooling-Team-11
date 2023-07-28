using CarPooling.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.DatabaseSeeder
{
    public static class UserSeeder
    {
        public static async Task UserSeeding(this IApplicationBuilder application)
        {
            using(var scope = application.ApplicationServices.CreateScope())
            {
                UserManager<User> _userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var userGosho = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "gosho@gmail.com",
                    FirstName = "Gosho",
                    LastName = "Goshev",
                    Email = "gosho@gmail.com",
                    AverageRating = 4.5m,
                    IsBlocked = false
                };
                var userPesho = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "pesho@gmail.com",
                    FirstName = "Pesho",
                    LastName = "Peshev",
                    Email = "pesho@gmail.com",
                    AverageRating = 4.5m,
                    IsBlocked = false
                };
                var userMisho = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "misho@gmail.com",
                    FirstName = "Misho",
                    LastName = "Mishev",
                    Email = "misho@gmail.com",
                    AverageRating = 4.5m,
                    IsBlocked = false
                };
                var password = "SamplePassword123!";
                var result1 = await _userManager.CreateAsync(userGosho, password);
                var result2 = await _userManager.CreateAsync(userPesho, password);
                var result3 = await _userManager.CreateAsync(userMisho, password);
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
            }
            
        }
    }
}
