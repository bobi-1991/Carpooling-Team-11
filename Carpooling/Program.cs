using CarPooling.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using CarPooling.Data.Data;
using FluentValidation.AspNetCore;
using CarPooling.Data.JsonManager;
using CarPooling.Data.Models;
using Carpooling.Infrastructure;
using Carpooling.BusinessLayer.Validation.Contracts;
using Carpooling.BusinessLayer.Validation;
using CarPooling.Data.Repositories.Contracts;
using CarPooling.Data.Repositories;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Services;
using FluentValidation;
using Newtonsoft.Json;
using CarPooling.Data.DatabaseSeeder;
using Carpooling.BusinessLayer.Helpers;
using CarPooling.BusinessLayer.Services;
using Carpooling.IAssemblyMarker;
using NToastNotify;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });

        ////DB
        builder.Services.AddDbContext<CarPoolingDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


        builder.Services.AddDefaultIdentity<User>()
            .AddRoles<UserRole>()
            .AddEntityFrameworkStores<CarPoolingDbContext>();

        //Helpers
        builder.Services.AddScoped<IJsonManager, JsonManager>();
        builder.Services.AddAutoMapper(typeof(Carpooling.Mapper));
        builder.Services.AddScoped<IIdentityHelper, IdentityHelper>();
        builder.Services.AddScoped<HttpClient>();

        // Add services to the container
        //  builder.Services.AddRazorPages();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ICarService, CarService>();
        builder.Services.AddScoped<IAddressService, AddressService>();
        builder.Services.AddScoped<ICountryService, CountryService>();
        builder.Services.AddScoped<IFeedbackService, FeedbackService>();
        builder.Services.AddScoped<ITravelService, TravelService>();
        builder.Services.AddScoped<ITripRequestService, TripRequestService>();
        builder.Services.AddScoped<IFileUploadService, FileUploadService>();
        builder.Services.AddScoped<IMapService, MapService>();
        //Add identity services
        builder.Services.AddScoped<UserManager<User>>();

        //Validators
        builder.Services.AddScoped<IUserValidation, UserValidation>();
        builder.Services.AddScoped<IAuthValidator, AuthValidator>();
        builder.Services.AddScoped<ITravelValidator, TravelValidator>();
        builder.Services.AddScoped<ITripRequestValidator, TripRequestValidator>();

        // Fluent Validation
        builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
        builder.Services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();

        //Add repositories to the container
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ITravelRepository, TravelRepository>();
        builder.Services.AddScoped<ITripRequestRepository, TripRequestRepository>();
        builder.Services.AddScoped<IAddressRepository, AddressRepository>();
        builder.Services.AddScoped<ICarRepository, CarRepository>();
        builder.Services.AddScoped<ICountryRepository, CountryRepository>();
        builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();




        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Carpooling API", Version = "v1" });
            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
        });

        var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins,
                              policy =>
                              {
                                  policy.WithOrigins("https://kit.fontawesome.com/")
                                  .AllowAnyHeader()
                                  .AllowAnyMethod();
                              });
        }); ;
        builder.Services.AddRouting(l => l.LowercaseUrls = true);
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
        await app.UserSeeding();
        app.SeedDatabaseTravel();

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCors(MyAllowSpecificOrigins);
        app.UseRouting();
        app.UseAuthentication();

        app.UseAuthorization();

        app.MapRazorPages();

        app.MapDefaultControllerRoute();

        app.Run();



    }
}