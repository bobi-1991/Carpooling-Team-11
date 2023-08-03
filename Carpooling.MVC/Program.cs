using Carpooling.BusinessLayer.Helpers;
using CarPooling.Data.Data;
using CarPooling.Data.JsonManager;
using CarPooling.Data.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Services;
using CarPooling.BusinessLayer.Services;
using Carpooling.BusinessLayer.Validation.Contracts;
using Carpooling.BusinessLayer.Validation;
using CarPooling.Data.Repositories.Contracts;
using CarPooling.Data.Repositories;
using FluentValidation.AspNetCore;
using FluentValidation;

namespace Carpooling.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
          //  builder.Services.AddControllersWithViews();

            builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            ////DB
            builder.Services.AddDbContext<CarPoolingDbContext>(options =>
                        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



            //builder.Services.AddDefaultIdentity<User>()
            //    .AddRoles<UserRole>()
            //    .AddEntityFrameworkStores<CarPoolingDbContext>();

            //Helpers
            builder.Services.AddScoped<IJsonManager, JsonManager>();
            builder.Services.AddAutoMapper(typeof(Carpooling.BusinessLayer.Helpers.Mapper));
            builder.Services.AddScoped<IIdentityHelper, IdentityHelper>();


            // Add services to the container
            //  builder.Services.AddRazorPages();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ICarService, CarService>();
            builder.Services.AddScoped<IAddressService, AddressService>();
            builder.Services.AddScoped<ICountryService, CountryService>();
            builder.Services.AddScoped<IFeedbackService, FeedbackService>();
            builder.Services.AddScoped<ITravelService, TravelService>();
            builder.Services.AddScoped<ITripRequestService, TripRequestService>();

            //Add identity services
            builder.Services.AddScoped<UserManager<User>>();

            //Validators
            builder.Services.AddScoped<IUserValidation, UserValidation>();
            builder.Services.AddScoped<IAuthValidator, AuthValidator>();
            builder.Services.AddScoped<ITravelValidator, TravelValidator>();
            builder.Services.AddScoped<ITripRequestValidator, TripRequestValidator>();

            // Fluent Validation
            builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            //
          //  builder.Services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();

            //Add repositories to the container
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ITravelRepository, TravelRepository>();
            builder.Services.AddScoped<ITripRequestRepository, TripRequestRepository>();
            builder.Services.AddScoped<IAddressRepository, AddressRepository>();
            builder.Services.AddScoped<ICarRepository, CarRepository>();
            builder.Services.AddScoped<ICountryRepository, CountryRepository>();
            builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();




            var app = builder.Build();

      
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //Session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(1000);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();

            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapDefaultControllerRoute();

            //app.MapControllerRoute(
            //    name: "default",
            //    pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}