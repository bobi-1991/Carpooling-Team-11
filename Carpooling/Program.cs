using CarPooling.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using CarPooling.Data.Data;
using Carpooling;
using CarPooling.Data.JsonManager;
using CarPooling.Data.Models;
using Carpooling.Infrastructure;
using Carpooling.BusinessLayer.Validation.Contracts;
using Carpooling.BusinessLayer.Validation;
using CarPooling.Data.Repositories.Contracts;
using CarPooling.Data.Repositories;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ////DB
        builder.Services.AddDbContext<CarPoolingDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddDefaultIdentity<User>()
            .AddRoles<UserRole>()
            .AddEntityFrameworkStores<CarPoolingDbContext>();

        //Helpers
        builder.Services.AddScoped<IJsonManager, JsonManager>();
        builder.Services.AddAutoMapper(typeof(Carpooling.BusinessLayer.Helpers.Mapper));

        // Add services to the container
        builder.Services.AddRazorPages();
        builder.Services.AddScoped<IUserService, UserService>();

        //Add identity services
        builder.Services.AddScoped<UserManager<User>>();



        //Validators
        builder.Services.AddScoped<IUserValidation, UserValidation>();

        //Add repositories to the container
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ITravelRepository, TravelRepository>();
        builder.Services.AddScoped<ITripRequestRepository, TripRequestRepository>();
       

        


        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Carpooling API", Version = "v1" });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Carpooling API V1");
            options.RoutePrefix = "api/swagger";
        });

        //Seed DB
        app.SeedDatabaseCountries();

        //Infrastructure
        app.UpdateDatabase();

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();

        app.UseAuthorization();

        app.MapRazorPages();

        app.Run();

    }
}