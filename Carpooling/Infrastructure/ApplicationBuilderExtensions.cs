using CarPooling.Data.Data;
using CarPooling.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Carpooling.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static void UpdateDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<CarPoolingDbContext>();
                context.Database.Migrate();

                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<UserRole>>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();

                Task.Run(async () =>
                {
                    var adminName = "Administrator";

                    var exists = await roleManager.RoleExistsAsync(adminName);

                    if (!exists)
                    {
                        await roleManager.CreateAsync(new UserRole
                        {
                            Name = adminName
                        });
                    }

                    var pasangerName = "Passenger";

                    var existsManager = await roleManager.RoleExistsAsync(pasangerName);

                    if (!existsManager)
                    {
                        await roleManager.CreateAsync(new UserRole
                        {
                            Name = pasangerName
                        });
                    }

                    var driverName = "Driver";

                    var existsMember = await roleManager.RoleExistsAsync(driverName);

                    if (!existsMember)
                    {
                        await roleManager.CreateAsync(new UserRole
                        {
                            Name = driverName
                        });
                    }

                    var adminUser = await userManager.FindByNameAsync(adminName);

                    if (adminUser == null)
                    {
                        adminUser = new User
                        {
                            UserName = "admin",
                            Email = "admin@admin.com"
                        };

                        var result = await userManager.CreateAsync(adminUser, "Admin12@");
                        //if (result.Succeeded)
                        //{

                            await userManager.AddToRoleAsync(adminUser, adminName);
                        //}
                        //else
                        //{
                        //    throw new ArgumentException($"{result.Errors.FirstOrDefault().Description}");
                        //}
                    }
                })
                .GetAwaiter()
                .GetResult();
            }
        }

    }
}
